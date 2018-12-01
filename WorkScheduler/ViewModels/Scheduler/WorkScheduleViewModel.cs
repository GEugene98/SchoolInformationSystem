using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkScheduler.ViewModels
{
    public class WorkScheduleViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ActivityViewModel Activity { get; set; }
        public AcademicYearViewModel AcademicYear { get; set; }
        public IEnumerable<ActionViewModel> Actions { get; set; }
        public UserViewModel User { get; set; }
    }
}
