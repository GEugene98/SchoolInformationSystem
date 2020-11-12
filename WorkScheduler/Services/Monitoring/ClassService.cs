using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WorkScheduler.Models.Monitoring.Shared;
using WorkScheduler.ViewModels;
using WorkScheduler.ViewModels.Monitoring.Shared;

namespace WorkScheduler.Services.Monitoring
{
    public class ClassService
    {
        protected Context Db;

        public ClassService(Context context)
        {
            Db = context;
        }

        public ClassVievModel GetClass(int studentId, int? academicYearId = null)
        {
            if (academicYearId == null)
            {
                var academicYear = Db.AcademicYears.FirstOrDefault(a => a.Start >= DateTime.Now && DateTime.Now <= a.End);

                if (academicYear == null)
                {
                    throw new Exception("There's no active academic years");
                }
                else
                {
                    academicYearId = academicYear.Id;
                }
            }

            var studentsQuery = Db.Students.Where(s => s.Id == studentId);
            var classStudentQuery = Db.ClassStudents;
            var classQuery =
                Db.Classes
                .Include(c => c.AcademicYear)
                .Where(c => c.AcademicYearId == academicYearId);

            var foundClass =
                (from s in studentsQuery
                 join cs in classStudentQuery on s.Id equals cs.StudentId
                 join c in classQuery on cs.ClassId equals c.Id
                 select new { Student = s, Class = c })
                .FirstOrDefault()?.Class;

            if (foundClass == null)
            {
                return null;
            }

            var classModel = new ClassVievModel
            {
                Id = foundClass.Id,
                Name = foundClass.Name,
                AcademicYear = new DictionaryViewModel<int>
                {
                    Id = foundClass.AcademicYear.Id,
                    Name = foundClass.AcademicYear.Name
                }
            };

            return classModel;
        }


        public int CreateClass(int academicYearId, int schoolId, string name)
        {
            var newClass = new Class
            {
                Name = name,
                AcademicYearId = academicYearId,
                SchoolId = schoolId
            };

            Db.Classes.Add(newClass);
            Db.SaveChanges();

            return newClass.Id;
        }

        

        //public IEnumerable<StudentViewModel> GetStudents()
        //{
        //    var studentsQuery = Db.Students;
        //    var classStudentQuery = Db.ClassStudents;
        //    var classQuery =
        //        Db.Classes
        //        .Include(c => c.AcademicYear)
        //        .Where(c => c.AcademicYearId == academicYearId);

        //    var joinResult =
        //        from s in studentsQuery
        //        join cs in classStudentQuery on s.Id equals cs.StudentId
        //        join c in classQuery on cs.ClassId equals c.Id
        //        select new { Student = s, Class = c };

        //    return Db.Students.Where(s => !s.IsDeleted)
        //        .ToList()
        //        .Select(s => new StudentViewModel
        //        {
        //            Id = s.Id,
        //            Class = new ClassVievModel
        //            {

        //            } 
        //        })
        //        ;
        //}

    }
}
