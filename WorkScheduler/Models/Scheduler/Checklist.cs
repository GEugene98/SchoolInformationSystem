using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkScheduler.Models.Base;
using WorkScheduler.Models.Identity;

namespace WorkScheduler.Models.Scheduler
{
    public class Checklist : Dictionary<int>
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public DateTime CreatedOn { get; set; }
        public string Comment { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
