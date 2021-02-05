using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using WorkScheduler.Models.Monitoring;
using WorkScheduler.Models.Scheduler;
using WorkScheduler.Models.Shared;

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

        public bool CanAccept { get; set; }
        public bool CanConfirm { get; set; }
        public bool CanUseChecklists { get; set; }
        public bool CanSeeAllChecklists { get; set; }
        public bool CanSeeAllProtocols { get; set; }
        public bool CanSeeAllSchedules { get; set; }

        public int? SchoolId { get; set; }
        public School School { get; set; }

        public virtual ICollection<WorkSchedule> WorkSchedules { get; set; }
        public virtual ICollection<ActionUser> ActionUsers { get; set; } // связь для установки ответственных
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<Checklist> Checklists { get; set; }
        public virtual ICollection<LoginLog> LoginLogs { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Contract> Contracts { get; set; }

    }
}
