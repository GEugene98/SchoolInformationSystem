using System;
using System.Collections.Generic;
using WorkScheduler.Models.Base;
using WorkScheduler.Models.Enums;
using WorkScheduler.Models.Monitoring.Shared;

namespace WorkScheduler.Models
{
    public class Action : Dictionary<int>
    {
        public Action()
        {
            ActionUsers = new List<ActionUser>();
        }

        public DateTime Date { get; set; }

        public int ConfirmationFormId { get; set; }
        public ConfirmationForm ConfirmationForm { get; set; }

        public int WorkScheduleId { get; set; }
        public WorkSchedule WorkSchedule { get; set; }

        public ActionStatus Status { get; set; } = ActionStatus.New;

        public bool IsDeleted { get; set; }

        public virtual ICollection<ActionUser> ActionUsers { get; set; }
        public virtual ICollection<StudentAction> StudentActions { get; set; }
        public virtual ICollection<Protocol> Protocols { get; set; }
    }
}
