using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WorkScheduler.Models.Monitoring.Shared;
using WorkScheduler.ViewModels;
using WorkScheduler.ViewModels.Monitoring.Shared;

namespace WorkScheduler.Services.Monitoring
{
    public class DictionaryService
    {
        protected Context Db;

        public DictionaryService(Context context)
        {
            Db = context;
        }

        public IEnumerable<Student> GetStudents()
        {
            return Db.Students.Where(s => !s.IsDeleted).ToList();
        }

        public IEnumerable<ClassVievModel> GetStudentsByClasses(int academicYearId)
        {
            var studentsQuery = Db.Students;
            var classStudentQuery = Db.ClassStudents;
            var classQuery =
                Db.Classes
                .Include(c => c.AcademicYear)
                .Where(c => c.AcademicYearId == academicYearId);

            var joinResult =
                from s in studentsQuery
                join cs in classStudentQuery on s.Id equals cs.StudentId
                join c in classQuery on cs.ClassId equals c.Id
                select new { Student = s, Class = c };

            var classes = new List<ClassVievModel>();
            var groups = joinResult.GroupBy(r => r.Class);

            foreach (var group in groups)
            {
                var classViewModel = new ClassVievModel
                {
                    Id = group.Key.Id,
                    Name = group.Key.Name,
                    AcademicYear = new DictionaryViewModel<int>
                    {
                        Id = group.Key.AcademicYear.Id,
                        Name = group.Key.AcademicYear.Name
                    },
                    Students = group.Select(g => new StudentViewModel
                    {
                        Id = g.Student.Id,
                        FirstName = g.Student.FirstName,
                        LastName = g.Student.LastName,
                        SurName = g.Student.SurName,
                        IsDeleted = g.Student.IsDeleted
                    })
                };

                classes.Add(classViewModel);
            }

            return classes;
        }

    }
}
