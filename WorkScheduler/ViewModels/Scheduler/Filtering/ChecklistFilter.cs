using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkScheduler.ViewModels.Scheduler.Filtering
{
    public class ChecklistFilter
    {
        public string Date { get; set; }
        public string Created { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public string UserId { get; set; }
    }
}
