﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkScheduler.Models.Base;
using WorkScheduler.Models.Shared;

namespace WorkScheduler.Models.Monitoring.Shared
{
    public class Class : Dictionary<int>
    {
        public int AcademicYearId { get; set; }
        public AcademicYear AcademicYear { get; set; }

        public int SchoolId { get; set; }
        public School School { get; set; }

        public virtual ICollection<ClassStudent> ClassStudents { get; set; }
    }
}
