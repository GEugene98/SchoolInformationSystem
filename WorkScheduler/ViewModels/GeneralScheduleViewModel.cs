using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkScheduler.ViewModels
{
    public class GeneralScheduleViewModel
    {
        public IEnumerable<Day> Days { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

    public class Day
    {
        public DateTime Date { get; set; }
        public string ShortDayOfWeekName { get; set; }
        public bool IsDayOff { get; set; }
        public IEnumerable<ActionViewModel> Actions { get; set; }
    }
}
