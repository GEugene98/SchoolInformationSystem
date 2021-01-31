using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkScheduler.ViewModels.Monitoring.Shared
{
    public class StudentViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SurName { get; set; }
        public string FullName { get; set; }

        public bool IsDeleted { get; set; }

        public int SchoolId { get; set; }

        public ClassVievModel Class { get; set; }
    }
}
