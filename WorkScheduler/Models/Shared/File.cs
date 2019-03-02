using System.Collections.Generic;
using WorkScheduler.Models.Base;
using WorkScheduler.Models.Monitoring.TalentedChildren;

namespace WorkScheduler.Models.Shared
{
    public class File : Dictionary<long>
    {
        public string Path { get; set; }
        public string Extension { get; set; }
        public double SizeMb { get; set; }

        public int? StudentAchivmentId { get; set; }
        public StudentAchivment StudentAchivment { get; set; }
    }
}
