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

        public List<FamilyViewModel> GetByGroup(int groupId)
        {
            var groupStudentsQuery = Db.GroupStudents
                .Where(gs => gs.GroupId == groupId);

            var families =
                from f in Db.Families
                join gs in groupStudentsQuery on f.StudentId equals gs.StudentId
                select f;

            return families
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
                    ClarifyFamilycomposition = f.ClarifyFamilyСomposition,
                    FamilyNumberChildren = f.FamilyNumberChildren,
                    Familycomposition = f.FamilyСomposition,
                    FamilyQualityLife = f.FamilyQualityLife,
                    RegistrationDate = f.RegistrationDate,
                    HealthGroup = f.HealthGroup,
                    PhysicalGroup = f.PhysicalGroup,
                    Registration = f.Registration,
                    Student = new StudentViewModel
                    {
                        FullName = f.Student.LastName + " " + f.Student.FirstName + " " + f.Student.SurName,
                        Birthday = f.Student.Birthday
                    },

                })
                .ToList();
        }

        public List<FamilyViewModel> Get(int schoolId, int classId)
        {
            var classStudentsQuery = Db.ClassStudents
                .Where(cs => cs.ClassId == classId && cs.Student.SchoolId == schoolId);

            var families =
                from f in Db.Families
                join cs in classStudentsQuery on f.StudentId equals cs.StudentId
                select f;

            return families
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
                    ClarifyFamilycomposition = f.ClarifyFamilyСomposition,
                    FamilyNumberChildren = f.FamilyNumberChildren,
                    Familycomposition = f.FamilyСomposition,
                    FamilyQualityLife = f.FamilyQualityLife,
                    RegistrationDate = f.RegistrationDate,
                    HealthGroup = f.HealthGroup,
                    PhysicalGroup = f.PhysicalGroup,
                    Registration = f.Registration,
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

        public void Create(FamilyViewModel family)
        {
            var newFamily = new Family();
            newFamily.BirthCertificate = family.BirthCertificate;
            newFamily.ClarifyFamilyСomposition = family.ClarifyFamilycomposition;
            newFamily.FamilyNumberChildren = family.FamilyNumberChildren;
            newFamily.FamilyQualityLife = family.FamilyQualityLife;
            newFamily.FamilyСomposition = family.Familycomposition;
            newFamily.FullNameFather = family.FullNameFather;
            newFamily.HealthGroup = family.HealthGroup;
            newFamily.FullNameMather = family.FullNameMather;
            newFamily.PassportNumber = family.PassportNumber;
            newFamily.PhoneFather = family.PhoneFather;
            newFamily.PhoneMother = family.PhoneMother;
            newFamily.PhysicalGroup = family.PhysicalGroup;
            newFamily.RegistrAddres = family.RegistrAddres;
            newFamily.Registration = family.Registration;
            newFamily.RegistrationDate = family.RegistrationDate;
            newFamily.ResidAddres = family.ResidAddres;
            newFamily.StudentId = family.Student.Id;
            newFamily.WorkFather = family.WorkFather;
            newFamily.WorkMother = family.WorkMother;
            Db.Families.Add(newFamily);
            try
            {
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key value violates unique constraint"))
                {
                    throw new Exception("Социальный паспорт, который вы создаете для выбранного ученика уже имеется в системе. Измените существующий паспорт или удалите его перед созданием нового");
                }
            }
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
