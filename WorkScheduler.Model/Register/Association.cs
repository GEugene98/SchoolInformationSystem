using System;
using System.Collections.Generic;
using WorkScheduler.Models.Base;
using WorkScheduler.Models.Enums;
using WorkScheduler.Models.Shared;

namespace WorkScheduler.Models.Register
{
    public class Association : Dictionary<int>
    {
        public int AcademicYearId { get; set; }
        public AcademicYear AcademicYear { get; set; }

        public AssociationType Type { get; set; }

        public int SchoolId { get; set; }
        public School School { get; set; }

        public virtual ICollection<AssociationGroup> AssociationGroups { get; set; }
        public virtual ICollection<PlaningRecord> PlaningRecords { get; set; }
    }
}
