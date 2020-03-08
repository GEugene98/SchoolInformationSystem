﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkScheduler.Models.Enums;
using WorkScheduler.ViewModels.Scheduler;

namespace WorkScheduler.ViewModels
{
    public class TicketViewModel : DictionaryViewModel<long>
    {
        public UserViewModel User { get; set; }
        public ActionViewModel Action { get; set; }
        public string Comment { get; set; }
        public string AssignmentComment { get; set; }
        public string ResponseComment { get; set; }
        public bool Done { get; set; }
        public bool Important { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Created { get; set; }
        public byte? Hours { get; set; }
        public byte? Minutes { get; set; }
        public ChecklistViewModel Checklist { get; set; }
        public TicketStatus? Status { get; set; }
        public bool HasChecklist { get; set; }
        public string UserId { get; set; }
        public int ChecklistId { get; set; }
        public bool IsExpiered { get; set; }
        public bool AutoGenerated { get; set; }
        public bool ActionIsOutOfDate { get; set; }
        public IEnumerable<FileViewModel> InFiles { get; set; }
        public IEnumerable<FileViewModel> OutFiles { get; set; }

        public IEnumerable<string> UserIdsToAssignTicket { get; set; }

        public bool Repeat { get; set; }
        public DateTime DateTo { get; set; }
        public IEnumerable<int> Days { get; set; }
    }
}
