using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkScheduler.ViewModels
{
    public class PeriodViewModel
    {
        public int StartDay { get; set; }
        public int StartMonth { get; set; }
        public int StartYear { get; set; }

        public int EndDay { get; set; }
        public int EndMonth  { get; set; }
        public int EndYear { get; set; }
    }
}
