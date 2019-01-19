using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkScheduler.ViewModels
{
    public class TicketPackViewModel
    {
        public DateTime Date { get; set; }
        public string DateToShow { get; set; }
        public IEnumerable<TicketTimeGroup> TimeGroups { get; set; }
    }

    public class TicketTimeGroup
    {
        public int Hour { get; set; }
        public IEnumerable<TicketViewModel> Tickets { get; set; }
    }
}
