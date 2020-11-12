using System;
using System.Collections.Generic;
using WorkScheduler.Models.Base;
using WorkScheduler.Models.Monitoring.Shared;
using WorkScheduler.Models.Register;

namespace WorkScheduler.Models
{
    public class AcademicYear : Dictionary<int>
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public virtual ICollection<WorkSchedule> WorkSchedules { get; set; }
        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<Association> Associations { get; set; }
        public virtual ICollection<PlaningRecord> PlaningRecords { get; set; }
    }
}
