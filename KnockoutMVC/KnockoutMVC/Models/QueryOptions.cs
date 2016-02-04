using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using KnockoutMVC.Models;

namespace KnockoutMVC.Models
{
    public class QueryOptions
    {
        public QueryOptions()
        {
            CurrentPage = 1;
            PageSize = 1;

            SortField = "Id";
            SortOrder = SortOrder.ASC;
        }
        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; }

        public string SortField { get; set; }

        public SortOrder SortOrder { get; set; }

        public string Sort
        {
            get
            {
                return string.Format("{0} {1}",
                SortField, SortOrder.ToString());
            }
        }
    }
}