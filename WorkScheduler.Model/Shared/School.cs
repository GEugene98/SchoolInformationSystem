using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkScheduler.Models.Base;
using WorkScheduler.Models.Identity;
using WorkScheduler.Models.Monitoring;
using WorkScheduler.Models.Monitoring.Shared;
using WorkScheduler.Models.Register;

namespace WorkScheduler.Models.Shared
{
    public class School : Dictionary<int>
    {
        public string ShortName { get; set; }
        public string Email { get; set; }
        public string DocumentHeaderHTML { get; set; }
        public string ActionNamesToMakeProtocolJSON { get; set; }

        public virtual ICollection<User> Users { get; set; }
        //public virtual ICollection<AcademicPeriod> AcademicPeriods { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<Association> Associations { get; set; }
        public virtual ICollection<Contract> Contracts { get; set; }

    }
}
