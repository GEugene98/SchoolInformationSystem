using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkScheduler.Models.Base;
using WorkScheduler.Models.Enums;
using WorkScheduler.Models.Identity;
using WorkScheduler.Models.Scheduler;

namespace WorkScheduler.Models
{
    public class Ticket : Dictionary<long>
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public int? ActionId { get; set; }
        public Action Action { get; set; }

        public string ChecklistId { get; set; }
        public Checklist Checklist { get; set; }

        public string Comment { get; set; }

        public string AssignmentComment { get; set; }

        public TicketStatus? Status { get; set; }

        public bool Done { get; set; }
        public bool Important { get; set; }

        public DateTime? Date { get; set; }
        public byte? Hours { get; set; }
        public byte? Minutes { get; set; }
    }
}
