﻿using System;
using System.Collections.Generic;
using WorkScheduler.Models.Base;
using WorkScheduler.Models.Identity;
using WorkScheduler.Models.Monitoring;
using WorkScheduler.Models.Shared;

namespace WorkScheduler.Models.Workflow
{
    public class IncomingDocument : Dictionary<int>
    {
        public int SchoolId { get; set; }
        public School School { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public string Num { get; set; }
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public string Type { get; set; }
        public DateTime? Taken { get; set; }
        public DateTime? Deadline { get; set; }
        public bool Done { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<IncomingDocumentFile> IncomingDocumentFiles { get; set; }
    }
}
