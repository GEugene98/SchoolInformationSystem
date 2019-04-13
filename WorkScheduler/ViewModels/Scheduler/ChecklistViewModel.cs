using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkScheduler.ViewModels.Scheduler
{
    public class ChecklistViewModel : DictionaryViewModel<int>
    {
        public UserViewModel User { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? Deadline { get; set; }
        public string Comment { get; set; }
        
        public int AssignedCount { get; set; }   
        public int AcceptedCount { get; set; } 
        public int DoneCount { get; set; }
        public int TotalCount { get; set; }     
        public int ExpieredCount { get; set; }     

        public IEnumerable<TicketViewModel> Tickets { get; set; }
    }
}
