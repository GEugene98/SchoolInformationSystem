using System;
using System.Collections.Generic;
using WorkScheduler.Models.Base;
using WorkScheduler.Models.Monitoring.TalentedChildren;
using WorkScheduler.Models.Scheduler;

namespace WorkScheduler.Models.Shared
{
    public class File : Dictionary<long>
    {
        public string Path { get; set; }
        public string Extension { get; set; }
        public double SizeMb { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public virtual ICollection<TicketFile> TicketFiles { get; set; }
    }
}
