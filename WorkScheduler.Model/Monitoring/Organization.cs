using System;
using System.Collections.Generic;
using WorkScheduler.Models.Base;
using WorkScheduler.Models.Shared;
using WorkScheduler.Models.Workflow;

namespace WorkScheduler.Models.Monitoring
{
    public class Organization : Dictionary<int>
    {
        public int SchoolId { get; set; }
        public School School { get; set; }

        public virtual ICollection<Contract> Contracts { get; set; }
        public virtual ICollection<IncomingDocument> IncomingDocuments { get; set; }
        public virtual ICollection<OutgoingDocument> OutgoingDocuments { get; set; }
    }
}
