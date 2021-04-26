using System;
using System.Collections.Generic;
using System.Linq;
using WorkScheduler.Models.Monitoring;
using WorkScheduler.Models.Monitoring.Shared;
using WorkScheduler.ViewModels.Monitoring;
using WorkScheduler.ViewModels.Monitoring.Shared;

namespace WorkScheduler.Services.Monitoring
{
    public class FamilyService
    {
        protected Context Db;

        public FamilyService(Context context)
        {
            Db = context;
        }

        public List<FamilyViewModel> Get(int schoolId)
        {
            return Db.Families
                .Where(f => f.Student.SchoolId == schoolId)
                .Select(f => new FamilyViewModel
                {
                    Id = f.Id,
                    PassportNumber = f.PassportNumber,
                    BirthCertificate = f.BirthCertificate,
                    RegistrAddres = f.RegistrAddres,
                    ResidAddres = f.ResidAddres,
                    FullNameMather = f.FullNameMather,
                    PhoneMother = f.PhoneMother,
                    WorkMother = f.WorkMother,
                    FullNameFather = f.FullNameFather,
                    PhoneFather = f.PhoneFather,
                    WorkFather = f.WorkFather,
                    Student = new StudentViewModel
                    {
                        FullName = f.Student.LastName + " " + f.Student.FirstName + " " + f.Student.SurName,
                        Birthday = f.Student.Birthday
                    },
                    
                })
                .ToList();
        }

        public void Update(Family family)
        {
            var foundFamily = Db.Families.FirstOrDefault(f => f.Id == family.Id);

            if (foundFamily == null)
            {
                throw new Exception("Записи с таким Id не найдено");
            }

            foundFamily.BirthCertificate = family.BirthCertificate;
            foundFamily.ClarifyFamilyСomposition = family.ClarifyFamilyСomposition;
            foundFamily.FamilyNumberChildren = family.FamilyNumberChildren;
            foundFamily.FamilyСomposition = family.FamilyСomposition;
            foundFamily.FullNameFather = family.FullNameFather;
            foundFamily.FullNameMather = family.FullNameMather;
            foundFamily.PassportNumber = family.PassportNumber;
            foundFamily.PhoneFather = family.PhoneFather;
            foundFamily.PhoneMother = family.PhoneMother;
            foundFamily.PhysicalGroup = family.PhysicalGroup;
            foundFamily.RegistrAddres = family.RegistrAddres;
            foundFamily.Registration = family.Registration;
            foundFamily.RegistrationDate = family.RegistrationDate;
            foundFamily.ResidAddres = family.ResidAddres;
            foundFamily.WorkFather = family.WorkFather;
            foundFamily.WorkMother = family.WorkMother;

            Db.SaveChanges();
        }

        public void Create(Family family)
        {
            Db.Families.Add(family);
            Db.SaveChanges();
        }

        public void Delete(int id)
        {
            var foundFamily = Db.Families.FirstOrDefault(f => f.Id == id);

            if (foundFamily == null)
            {
                throw new Exception("Записи с таким Id не найдено");
            }

            Db.Families.Remove(foundFamily);

            Db.SaveChanges();
        }
    }
}
