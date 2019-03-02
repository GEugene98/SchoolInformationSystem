using System.Collections.Generic;
using WorkScheduler.Models.Base;
using WorkScheduler.Models.Monitoring.TalentedChildren;

namespace WorkScheduler.Models.Monitoring.Shared
{
    public class AchivmentLevel : Dictionary<int>
    {
        public virtual ICollection<StudentAchivment> StudentAchivments { get; set; }
    }
}
