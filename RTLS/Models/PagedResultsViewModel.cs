using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTLS.Models
{
    public class PagedResultsViewModel
    {
        public PagedResultsViewModel()
        {
            lstMacAddress = new List<Device>();
        }
        public int currentPageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int PageRange { get; set; }
        public List<Device> lstMacAddress { get; set; }
    }
}