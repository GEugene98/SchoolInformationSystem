using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkScheduler.Models.Base;
using WorkScheduler.Models.Register;
using WorkScheduler.Models.Shared;

namespace WorkScheduler.Models.Monitoring.Shared
{
    public class Student : EntityBase<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SurName { get; set; }

        public bool IsDeleted { get; set; }

        public int SchoolId { get; set; }
        public School School { get; set; }

        public virtual ICollection<ClassStudent> ClassStudents { get; set; }
        public virtual ICollection<GroupStudent> GroupStudents { get; set; }
        public virtual ICollection<RegisterRecord> RegisterRecords { get; set; }
    }
}
