using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkScheduler.Models.Base;

namespace WorkScheduler.Models.Monitoring.Shared
{
    public class GroupStudent : EntityBase<int>
    {
        public int GroupId { get; set; }
        public Group Group { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }
    }
}