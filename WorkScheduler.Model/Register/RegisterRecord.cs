using System;
using WorkScheduler.Models.Base;
using WorkScheduler.Models.Monitoring.Shared;

namespace WorkScheduler.Models.Register
{
    public class RegisterRecord: EntityBase<int>
    {
        public string Content { get; set; }

        public long PlaningRecordId { get; set; }
        public PlaningRecord PlaningRecord { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }
    }
}
