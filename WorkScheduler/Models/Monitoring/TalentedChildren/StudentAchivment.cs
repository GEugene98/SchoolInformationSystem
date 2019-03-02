using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkScheduler.Models.Base;
using WorkScheduler.Models.Monitoring.Shared;
using WorkScheduler.Models.Shared;

namespace WorkScheduler.Models.Monitoring.TalentedChildren
{
    public class StudentAchivment : Dictionary<int>
    {
        public DateTime Date { get; set; }

        public int StudentActionId { get; set; }
        public StudentAction StudentAction { get; set; }

        public int AchivmentLevelId { get; set; }
        public AchivmentLevel AchivmentLevel { get; set; }

        public int AchivmentResultId { get; set; }
        public AchivmentResult AchivmentResult { get; set; }

        public virtual ICollection<File> Files { get; set; }
    }
}
