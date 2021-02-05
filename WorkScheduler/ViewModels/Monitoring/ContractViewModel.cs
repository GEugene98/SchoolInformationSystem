using System;
using WorkScheduler.Models.Monitoring.Enums;

namespace WorkScheduler.ViewModels.Monitoring
{
    public class ContractViewModel
    {
        public long Id { get; set; }

        public int OrganizationId { get; set; }
        public OrganizationViewModel Organization { get; set; }

        public string Number { get; set; }
        public DateTime SigningDate { get; set; }
        public string Subject { get; set; }

        public string SignedById { get; set; }
        public UserViewModel SignedBy { get; set; }

        public decimal Sum { get; set; }
        public ContractStatus? Status { get; set; }
        public DateTime ControlDate { get; set; }
        public string Comment { get; set; }

        public int SchoolId { get; set; }
        public DictionaryViewModel<int> School { get; set; }
    }
}
