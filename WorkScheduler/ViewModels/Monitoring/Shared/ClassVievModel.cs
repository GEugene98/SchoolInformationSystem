using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkScheduler.ViewModels.Monitoring.Shared
{
    public class ClassVievModel : DictionaryViewModel<int>
    {
        public DictionaryViewModel<int> AcademicYear { get; set; }
        public IEnumerable<StudentViewModel> Students { get; set; }
    }
}
