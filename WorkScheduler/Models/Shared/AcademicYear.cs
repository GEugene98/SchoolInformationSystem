using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkScheduler.Models.Base;

namespace WorkScheduler.Models
{
    public class AcademicYear : Dictionary<int>
    {
        public virtual ICollection<WorkSchedule> WorkSchedules { get; set; }
    }
}
