using System;
using System.Collections.Generic;
using WorkScheduler.Models.Enums;

namespace WorkScheduler.ViewModels.Register
{
    public class AssociationViewModel : DictionaryViewModel<int>
    {
        public DictionaryViewModel<int> AcademicYear { get; set; }
        public AssociationType Type { get; set; }

        public UserViewModel User;

        public List<GroupViewModel> Groups { get; set; }
    }
}
