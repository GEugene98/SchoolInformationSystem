using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkScheduler.Models.Identity
{
    public class User : IdentityUser
    {
        public User() : base()
        {
            ActionUsers = new List<ActionUser>();
            Tickets = new List<Ticket>();
            WorkSchedules = new List<WorkSchedule>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SurName { get; set; }
        public bool GetNotifications { get; set; }

        public virtual ICollection<WorkSchedule> WorkSchedules { get; set; }
        public virtual ICollection<ActionUser> ActionUsers { get; set; } // связь для установки ответственных
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
