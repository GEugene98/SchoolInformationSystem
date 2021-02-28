using System;
using System.Collections.Generic;
using WorkScheduler.ViewModels.Monitoring.Shared;

namespace WorkScheduler.ViewModels.Register
{
    public class RegisterRow
    {
        public StudentViewModel Student { get; set; }
        public List<RegisterRecordViewModel> Cells { get; set; }
    }
}
