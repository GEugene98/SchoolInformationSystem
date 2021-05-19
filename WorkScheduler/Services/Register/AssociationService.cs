using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WorkScheduler.Models.Enums;
using WorkScheduler.Models.Monitoring.Shared;
using WorkScheduler.Models.Register;
using WorkScheduler.ViewModels.Monitoring.Shared;
using WorkScheduler.ViewModels.Register;

namespace WorkScheduler.Services.Register
{
    public class AssociationService
    {
        protected Context Db;

        public AssociationService(Context context)
        {
            Db = context;
        }

        public AssociationViewModel Get(int id)
        {
            var found = Db.Associations.Include(a => a.AcademicYear).FirstOrDefault(a => a.Id == id);
            var ass = new AssociationViewModel();
            ass.Id = found.Id;
            ass.Name = found.Name;
            ass.Type = found.Type;
            ass.AcademicYear = new ViewModels.DictionaryViewModel<int> { Name = found.AcademicYear.Name, Id = found.AcademicYear.Id };
            return ass;
        }

        public List<AssociationViewModel> GetAssociations(AssociationType type, int schoolId, int academicYearId)
        {
            var associationsQuery = Db.Associations
                .Where(a => a.AcademicYearId == academicYearId && a.SchoolId == schoolId && a.Type == type);

            var joinResultQuery =
                (
                    from a in associationsQuery
                    join ag in Db.AssociationGroups on a.Id equals ag.AssociationId
                    join g in Db.Groups on ag.GroupId equals g.Id
                    select new { Association = a, Group = g }
                );


            var associationsAndGroups = joinResultQuery.ToList();

            var groupsQuery = joinResultQuery.Select(j => j.Group);

            var students =
                (
                    from g in groupsQuery
                    join gs in Db.GroupStudents on g.Id equals gs.GroupId
                    join s in Db.Students on gs.StudentId equals s.Id
                    select new
                    {
                        GroupId = g.Id,
                        Student = new StudentViewModel
                        {
                            Id = s.Id,
                            FullName = s.LastName + " " + s.FirstName + " " + s.SurName
                        }
                    }
                ).ToList();

            var associations = associationsQuery
                .OrderBy(a => a.Name)
                .ToList()
                .Select(a => new AssociationViewModel
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Groups = associationsAndGroups
                            .Where(jr => jr.Association.Id == a.Id)
                            .Select(jr => new GroupViewModel {
                                Id = jr.Group.Id,
                                Name = jr.Group.Name,
                                Students = students.Where(s => s.GroupId == jr.Group.Id).Select(s => s.Student).ToList()
                            }).ToList()
                    })
                .ToList();

            return associations;
        }

        public void DeleteAssociation(int id)
        {
            var found = Db.Associations.FirstOrDefault(a => a.Id == id);

            if (found == null)
            {
                throw new Exception("Запись не найдена. Обновите страницу");
            }
            Db.Associations.Remove(found);
            Db.SaveChanges();
        }

        public int CreateAssociation(AssociationViewModel association, int schoolId, int academicYearId)
        {
            var newAssociation = new Association();
            newAssociation.AcademicYearId = academicYearId;
            newAssociation.SchoolId = schoolId;
            newAssociation.Name = association.Name;
            newAssociation.Type = association.Type;

            Db.Associations.Add(newAssociation);
            Db.SaveChanges();

            if (association.Groups != null && association.Groups.Count > 0)
            {
                foreach (var item in association.Groups)
                {
                    int groupId;

                    if (item.Id.HasValue)
                    {
                        var foundGroup = Db.Groups.FirstOrDefault(g => g.Id == item.Id);
                        groupId = foundGroup.Id;

                    }
                    else
                    {
                        var newGroup = new Group
                        {
                            Name = item.Name,
                            AcademicYearId = academicYearId,
                            SchoolId = schoolId,
                            Type = association.Type
                        };

                        Db.Groups.Add(newGroup);
                        Db.SaveChanges();

                        groupId = newGroup.Id;
                    }

                    var newAssotiationGroup = new AssociationGroup { AssociationId = newAssociation.Id, GroupId = groupId };

                    Db.AssociationGroups.Add(newAssotiationGroup);
                    Db.SaveChanges();

                    if (item.Students != null && item.Students.Count > 0)
                    {
                        var gss = new List<GroupStudent>();

                        foreach (var st in item.Students)
                        {
                            if (Db.GroupStudents.FirstOrDefault(gs => gs.StudentId == st.Id && gs.GroupId == groupId) == null)
                            {
                                gss.Add(new GroupStudent { GroupId = groupId, StudentId = st.Id });
                            }
                        }

                        Db.GroupStudents.AddRange(gss);
                        Db.SaveChanges();
                    }

                }

                


            }
           

            return newAssociation.Id;
        }
    }
}
