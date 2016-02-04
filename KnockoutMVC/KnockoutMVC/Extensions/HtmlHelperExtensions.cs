using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Web.Mvc;
using KnockoutMVC.DAL;
using KnockoutMVC.Models;

public static class HtmlHelperExtensions
{
    #region // Convert Model Into Json
    public static HtmlString HtmlConvertToJson(this HtmlHelper htmlHelper, object model)
    {
        var settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented
        };
        return new HtmlString(JsonConvert.SerializeObject(model, settings));
    }
    #endregion

    #region// Sorting 
    public static MvcHtmlString BuildSortableLink(this HtmlHelper htmlHelper, string fieldName, string actionName, string sortField, QueryOptions queryOptions)
    {
        var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
        var isCurrentSortField = queryOptions.SortField == sortField;
        return new MvcHtmlString(string.Format("<a href=\"{0}\">{1} {2}</a>", urlHelper.Action(actionName,
        new
        {
            SortField = sortField,
            SortOrder = (isCurrentSortField
        && queryOptions.SortOrder == SortOrder.ASC)
        ? SortOrder.DESC : SortOrder.ASC
        }),
        fieldName,
        BuildSortIcon(isCurrentSortField, queryOptions)));
    }

    private static string BuildSortIcon(bool isCurrentSortField, QueryOptions queryOptions)
    {
        string sortIcon = "sort";
        if (isCurrentSortField)
        {
            sortIcon += "-by-alphabet";
            if (queryOptions.SortOrder == SortOrder.DESC)
                sortIcon += "-alt";
        }
        return string.Format("<span class=\"{0} {1}{2}\"></span>",
        "glyphicon", "glyphicon-", sortIcon);
    }
    #endregion

    #region// Paging
    public static MvcHtmlString BuildNextPreviousLinks(this HtmlHelper htmlHelper, QueryOptions queryOptions, string actionName)
    {
        var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

        return new MvcHtmlString(string.Format(
        "<nav>" +
        " <ul class=\"pager\">" +
        " <li class=\"previous {0}\">{1}</li>" +
        " <li class=\"next {2}\">{3}</li>" +
        " </ul>" +
        "</nav>",
        IsPreviousDisabled(queryOptions),
        BuildPreviousLink(urlHelper, queryOptions, actionName),
        IsNextDisabled(queryOptions),
        BuildNextLink(urlHelper, queryOptions, actionName)
        ));
    }

    private static string IsPreviousDisabled(QueryOptions queryOptions)
    {
        return (queryOptions.CurrentPage == 1)
        ? "disabled" : string.Empty;
    }

    private static string IsNextDisabled(QueryOptions queryOptions)
    {
        return (queryOptions.CurrentPage == queryOptions.TotalPages)
        ? "disabled" : string.Empty;
    }

    private static string BuildPreviousLink(UrlHelper urlHelper, QueryOptions queryOptions, string actionName)
    {
        return string.Format(
        "<a href=\"{0}\"><span aria-hidden=\"true\">&larr;</span> Previous</a>",
        urlHelper.Action(actionName, new
        {
            SortOrder = queryOptions.SortOrder,
            SortField = queryOptions.SortField,
            CurrentPage = queryOptions.CurrentPage - 1,
            PageSize = queryOptions.PageSize - 1
        }));
    }

    private static string BuildNextLink(UrlHelper urlHelper, QueryOptions queryOptions, string actionName)
    {
        return string.Format(
        "<a href=\"{0}\">Next <span aria-hidden=\"true\">&rarr;</span></a>",
        urlHelper.Action(actionName, new
        {
            SortOrder = queryOptions.SortOrder,
            SortField = queryOptions.SortField,
            CurrentPage = queryOptions.CurrentPage + 1,
            PageSize = queryOptions.PageSize + 1
        }));
    }
}
    #endregion