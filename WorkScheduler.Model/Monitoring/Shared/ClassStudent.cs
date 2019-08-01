using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkScheduler.Models.Monitoring.Shared
{
    public class ClassStudent
    {
        public int ClassId { get; set; }
        public Class Class { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }
    }
}
