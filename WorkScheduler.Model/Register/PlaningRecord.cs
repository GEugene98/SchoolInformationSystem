using System;
using System.Collections.Generic;
using WorkScheduler.Models.Base;
using WorkScheduler.Models.Monitoring.Shared;
using WorkScheduler.Models.Shared;

namespace WorkScheduler.Models.Register
{
    public class PlaningRecord : Dictionary<long>
    {
        public DateTime? Date { get; set; }
        public string Hours { get; set; }
        public string Comment { get; set; }

        public int AssociationId { get; set; }
        public Association Association { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }

        public int AcademicYearId { get; set; }
        public AcademicYear AcademicYear { get; set; }

        //public int AcademicPeriodId { get; set; }
        //public AcademicPeriod AcademicPeriod { get; set; }

        public virtual ICollection<RegisterRecord> RegisterRecords { get; set; }
    }
}
