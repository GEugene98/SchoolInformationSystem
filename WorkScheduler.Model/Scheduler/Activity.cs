using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkScheduler.Models.Base;
using WorkScheduler.Models.Enums;

namespace WorkScheduler.Models
{
    public class Activity : Dictionary<int>
    {
        public Color Color { get; set; } = Color.White;
        public virtual ICollection<WorkSchedule> WorkSchedules { get; set; }
        public int Priority { get; set; }
    }
}
