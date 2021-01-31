﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkScheduler.Models.Base;
using WorkScheduler.Models.Register;
using WorkScheduler.Models.Shared;

namespace WorkScheduler.Models.Monitoring.Shared
{
    public class Group : Dictionary<int>
    {
        public int SchoolId { get; set; }
        public School School { get; set; }

        public virtual ICollection<GroupStudent> GroupStudents { get; set; }
        public virtual ICollection<AssociationGroup> AssociationGroups { get; set; }
        public virtual ICollection<PlaningRecord> PlaningRecords { get; set; }
    }
}