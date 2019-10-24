using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkScheduler.ViewModels
{
    public class RequestDetails<FilterType>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }

        public string SortProperty { get; set; }
        public SortDirection SortDirection { get; set; }

        public FilterType Filter { get; set; }
    }

}
