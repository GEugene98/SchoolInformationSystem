using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WorkScheduler.Models.Monitoring.Shared;
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

        public int CreateStudent(StudentViewModel student, int? classId)
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

            var cs = new ClassStudent
            {
                ClassId = classId,
                StudentId = studentId
            };

            Db.ClassStudents.Add(cs);

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
            var classStudents = Db.ClassStudents.Where(cs => cs.Student.SchoolId == schoolId);


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
