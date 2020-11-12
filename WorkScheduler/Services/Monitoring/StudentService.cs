using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WorkScheduler.Models.Monitoring.Shared;
using WorkScheduler.ViewModels;
using WorkScheduler.ViewModels.Monitoring.Shared;

namespace WorkScheduler.Services.Monitoring
{
    public class StudentService
    {
        protected Context Db;

        public StudentService(Context context)
        {
            Db = context;
        }

        public int CreateStudent(StudentViewModel student, int? classId = null)
        {
            var newStudent = new Student
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                SurName = student.SurName,
                SchoolId = student.SchoolId
            };

            Db.Students.Add(newStudent);
            Db.SaveChanges();

            if (classId.HasValue)
            {
                var cs = new ClassStudent
                {
                    ClassId = classId.Value,
                    StudentId = newStudent.Id
                };

                Db.ClassStudents.Add(cs);
                Db.SaveChanges();
            }

            return newStudent.Id;
        }

        public void ExcludeFromClass(int studentId)
        {
            var cStudent = Db.ClassStudents.FirstOrDefault(cs => cs.StudentId == studentId);

            if (cStudent == null)
            {
                throw new Exception("There's no student in the class");
            }

            Db.ClassStudents.Remove(cStudent);
            Db.SaveChanges();
        }

        public void PutToClass(int studentId, int classId)
        {
            var cStudent = Db.ClassStudents.FirstOrDefault(cs => cs.StudentId == studentId);

            if (cStudent != null)
            {
                Db.ClassStudents.Remove(cStudent);
            }

            var newCs = new ClassStudent
            {
                ClassId = classId,
                StudentId = studentId
            };

            Db.ClassStudents.Add(newCs);

            Db.SaveChanges();
        }

        public void PutStudentsToClass(IEnumerable<int> studentIds, int classId)
        {
            var cStudents = Db.ClassStudents.Where(cs => studentIds.Contains(cs.StudentId));

            if (cStudents.Count() > 0)
            {
                Db.ClassStudents.RemoveRange(cStudents);
            }

            foreach (var id in studentIds)
            {
                var cs = new ClassStudent
                {
                    ClassId = classId,
                    StudentId = id
                };

                Db.ClassStudents.Add(cs);
            }
      
            Db.SaveChanges();
        }

        public IEnumerable<StudentViewModel> GetAllStudents(int schoolId)
        {
            return
                Db.Students
                    .Where(s => s.SchoolId == schoolId)
                    .OrderBy(s => s.LastName)
                    .Select(s => new StudentViewModel
                                    {
                                        Id = s.Id,
                                        SchoolId = s.SchoolId,
                                        FirstName = s.FirstName,
                                        LastName = s.LastName,
                                        SurName = s.SurName
                                    }
                            )
                    .ToList();
        }

        public IEnumerable<StudentViewModel> GetStudentsWithoutClass(int schoolId)
        {
            var allStudents = Db.Students
                .Where(s => s.SchoolId == schoolId)
                .ToList();

            var studentsWithClass = Db.ClassStudents
                .Where(cs => cs.Student.SchoolId == schoolId)
                .Select(cs => cs.Student)
                .ToList();

            return allStudents
                .Where(s => studentsWithClass.FirstOrDefault(swc => swc.Id == s.Id) == null)
                .OrderBy(s => s.LastName)
                .Select(s => new StudentViewModel
                {
                     Id = s.Id,
                     SchoolId = s.SchoolId,
                     FirstName = s.FirstName,
                     LastName = s.LastName,
                     SurName = s.SurName
                });
        }

        public IEnumerable<ClassVievModel> GetStudentsByClasses(int academicYearId, int schoolId)
        {
            var studentsQuery = Db.Students.Where(s => s.SchoolId == schoolId);
            var classStudentQuery = Db.ClassStudents;
            var classQuery =
                Db.Classes
                .Include(c => c.AcademicYear)
                .Where(c => c.AcademicYearId == academicYearId && c.SchoolId == schoolId);

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

        public void AchiveStudent(int studentId)
        {
            var student = Db.Students.FirstOrDefault(s => s.Id == studentId);

            if (student == null)
            {
                throw new Exception();
            }

            student.IsDeleted = true;

            Db.SaveChanges();
        }

        public void DeleteStudent(int studentId)
        {
            var student = Db.Students.FirstOrDefault(s => s.Id == studentId);

            if (student == null)
            {
                throw new Exception();
            }

            Db.Remove(student);

            Db.SaveChanges();
        }
    }
}
