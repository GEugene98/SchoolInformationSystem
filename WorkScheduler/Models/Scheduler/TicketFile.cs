using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkScheduler.Models.Base;
using WorkScheduler.Models.Shared;

namespace WorkScheduler.Models.Scheduler
{
    public class TicketFile : EntityBase<long>
    {
        public long TicketId { get; set; }
        public Ticket Ticket { get; set; }

        public long FileId { get; set; }
        public File File { get; set; }

        public DateTime Created { get; set; }
    }
}
