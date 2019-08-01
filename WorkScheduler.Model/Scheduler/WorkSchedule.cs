using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkScheduler.Models.Base;
using WorkScheduler.Models.Identity;

namespace WorkScheduler.Models
{
    public class WorkSchedule : EntityBase<int>
    {
        public string Name { get; set; }

        public int ActivityId { get; set; }
        public Activity Activity { get; set; }

        public int AcademicYearId { get; set; }
        public AcademicYear AcademicYear { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public virtual ICollection<Action> Actions { get; set; }
    }
}
