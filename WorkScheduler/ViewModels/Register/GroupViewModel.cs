using System;
using System.Collections.Generic;
using WorkScheduler.Models.Enums;
using WorkScheduler.ViewModels.Monitoring.Shared;

namespace WorkScheduler.ViewModels.Register
{
    public class GroupViewModel : DictionaryViewModel<int?>
    {
        public List<StudentViewModel> Students { get; set; }
        public AssociationType Type { get; set; }
        public DictionaryViewModel<int> AcademicYear { get; set; }
    }
}
