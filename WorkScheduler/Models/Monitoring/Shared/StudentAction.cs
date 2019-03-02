using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkScheduler.Models.Base;
using WorkScheduler.Models.Monitoring.TalentedChildren;

namespace WorkScheduler.Models.Monitoring.Shared
{
    public class StudentAction : Dictionary<int>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        //Мероприятие ученика может иметь логическую связь с мероприятию учителя
        public int? ActionId { get; set; }
        public Action Action { get; set; }

        public virtual ICollection<StudentAchivment> StudentAchivments { get; set; }
    }
}
