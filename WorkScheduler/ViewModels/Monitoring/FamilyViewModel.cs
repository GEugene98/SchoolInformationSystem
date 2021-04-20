using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkScheduler.Models.Monitoring.Enums.Family;
using WorkScheduler.ViewModels.Monitoring.Shared;

namespace WorkScheduler.ViewModels.Monitoring
{
    public class FamilyViewModel
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public StudentViewModel Student { get; set; }

        public string PassportNumber { get; set; }
        public string BirthCertificate { get; set; }
        public string RegistrAddres { get; set; }
        public string ResidAddres { get; set; }
        public string FullNameMather { get; set; }
        public string PhoneMother { get; set; }
        public string WorkMother { get; set; }
        public string FullNameFather { get; set; }
        public string PhoneFather { get; set; }
        public string WorkFather { get; set; }

        public FamilyСomposition FamilyСomposition { get; set; }
        public ClarifyFamilyСomposition ClarifyFamilyСomposition { get; set; }
        public FamilyNumberChildren FamilyNumberChildren { get; set; }
        public PhysicalGroup PhysicalGroup { get; set; }
        public Registration Registration { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}
