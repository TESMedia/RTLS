using System.Collections.Generic;

namespace RTLS.Domains
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