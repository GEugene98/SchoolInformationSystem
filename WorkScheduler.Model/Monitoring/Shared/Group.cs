using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkScheduler.Models.Base;

namespace WorkScheduler.Models.Monitoring.Shared
{
    public class Group : Dictionary<int>
    {
        public virtual ICollection<GroupStudent> GroupStudents { get; set; }
    }
}