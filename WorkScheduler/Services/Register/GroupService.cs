using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WorkScheduler.Models.Enums;
using WorkScheduler.Models.Monitoring.Shared;
using WorkScheduler.ViewModels.Monitoring.Shared;
using WorkScheduler.ViewModels.Register;

namespace WorkScheduler.Services.Register
{
    public class GroupService
    {
        protected Context Db;

        public GroupService(Context context)
        {
            Db = context;
        }

        public GroupViewModel Get(int id)
        {
            var found = Db.Groups.Include(g => g.AcademicYear).FirstOrDefault(g => g.Id == id);
            var gr = new GroupViewModel();
            gr.Id = found.Id;
            gr.Name = found.Name;
            gr.Type = found.Type;
            gr.AcademicYear = new ViewModels.DictionaryViewModel<int> { Id = found.AcademicYear.Id, Name = found.AcademicYear.Name };
            return gr;
        }

        public List<GroupViewModel> GetGroups(int academicYearId, int schoolId, AssociationType type)
        {
            var groupQuery = Db.Groups.Where(g => g.SchoolId == schoolId && g.AcademicYearId == academicYearId && g.Type == type);

            var joinResult =
                (
                    from g in groupQuery
                    join gs in Db.GroupStudents on g.Id equals gs.GroupId
                    join s in Db.Students on gs.StudentId equals s.Id
                    select new { Group = g, Student = s }
                ).ToList();

            return groupQuery
                .ToList()
                .Select(g => new GroupViewModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    Students = joinResult.Where(jr => jr.Group.Id == g.Id).Select(jr => new StudentViewModel
                    {
                        Id = jr.Student.Id,
                        FullName = jr.Student.LastName + " " + jr.Student.FirstName + " " + jr.Student.SurName,
                    })
                    .ToList()
                })
                .ToList();

        }

        public int CreateGroup(GroupViewModel group, int academicYearId, int schoolId)
        {
            var newGroup = new Group();
            newGroup.AcademicYearId = academicYearId;
            newGroup.SchoolId = schoolId;
            newGroup.Name = group.Name;
            newGroup.Type = group.Type;

            Db.Groups.Add(newGroup);
            Db.SaveChanges();

            if (group.Students != null && group.Students.Count > 0)
            {
                var gss = new List<GroupStudent>();

                foreach (var item in group.Students)
                    gss.Add(new GroupStudent { GroupId = newGroup.Id, StudentId = item.Id });

                Db.GroupStudents.AddRange(gss);
                Db.SaveChanges();
            }

            return newGroup.Id;
        }

        public void UpdateGroup(GroupViewModel group)
        {
            var foundGroup = Db.Groups.FirstOrDefault(g => g.Id == group.Id);

            foundGroup.Name = group.Name;

            Db.SaveChanges();

            var bindings = Db.GroupStudents.Where(gs => gs.GroupId == group.Id);
            Db.GroupStudents.RemoveRange(bindings);

            if (group.Students != null && group.Students.Count > 0)
            {
                var gss = new List<GroupStudent>();

                foreach (var item in group.Students)
                    gss.Add(new GroupStudent { GroupId = foundGroup.Id, StudentId = item.Id });

                Db.GroupStudents.AddRange(gss);
                Db.SaveChanges();
            }

        }

    }
}
