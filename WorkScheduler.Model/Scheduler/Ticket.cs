﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkScheduler.Models.Base;
using WorkScheduler.Models.Enums;
using WorkScheduler.Models.Identity;
using WorkScheduler.Models.Scheduler;
using WorkScheduler.Models.Workflow;

namespace WorkScheduler.Models
{
    public class Ticket : Dictionary<long>
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public int? ActionId { get; set; }
        public Action Action { get; set; }

        public int? IncomingDocumentId { get; set; }
        public IncomingDocument IncomingDocument { get; set; }

        public int? OutgoingDocumentId { get; set; }
        public OutgoingDocument OutgoingDocument { get; set; }

        public bool OnCheck { get; set; }

        public int? ChecklistId { get; set; }
        public Checklist Checklist { get; set; }

        public string Comment { get; set; }

        public string AssignmentComment { get; set; }
        public string ResponseComment { get; set; }

        public TicketStatus? Status { get; set; }

        public bool Done { get; set; }
        public bool Important { get; set; }
        public bool AutoGenerated { get; set; }
        public bool Notify { get; set; }

        public DateTime? Date { get; set; } // Дата исполнения в чек-листе
        public DateTime Created { get; set; } = DateTime.Now;
        public byte? Hours { get; set; }
        public byte? Minutes { get; set; }

        public virtual ICollection<TicketFile> TicketFiles { get; set; }
    }
}
