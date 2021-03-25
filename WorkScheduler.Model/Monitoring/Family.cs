using System;
using System.Collections.Generic;
using System.Text;
using WorkScheduler.Models.Base;
using WorkScheduler.Models.Monitoring.Enums.Family;
using WorkScheduler.Models.Monitoring.Shared;

namespace WorkScheduler.Models.Monitoring
{
    public class Family : EntityBase<int>
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }

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
