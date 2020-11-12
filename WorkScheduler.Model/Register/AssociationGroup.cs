using System;
using WorkScheduler.Models.Monitoring.Shared;

namespace WorkScheduler.Models.Register
{
    public class AssociationGroup
    {
        public int GroupId { get; set; }
        public Group Group { get; set; }

        public int AssociationId { get; set; }
        public Association Association { get; set; }
    }
}
