using System;
using WorkScheduler.Models.Base;
using WorkScheduler.Models.Identity;
using WorkScheduler.Models.Monitoring.Enums;
using WorkScheduler.Models.Shared;

namespace WorkScheduler.Models.Monitoring
{
    public class Contract : EntityBase<long>
    {
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }

        public string Number { get; set; }
        public DateTime SigningDate { get; set; }
        public string Subject { get; set; }

        public string SignedById { get; set; }
        public User SignedBy { get; set; }

        public decimal Sum { get; set; }
        public ContractStatus? Status { get; set; }
        public DateTime ControlDate { get; set; }
        public string Comment { get; set; }

        public int SchoolId { get; set; }
        public School School { get; set; }

    }
}
