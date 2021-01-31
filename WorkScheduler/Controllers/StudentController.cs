using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkScheduler.Services.Monitoring;
using WorkScheduler.ViewModels.Monitoring.Shared;

namespace WorkScheduler.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class StudentController : Controller
    {
        protected Context Db;
        protected StudentService StudentService;

        public StudentController(Context context, StudentService studentService)
        {
            Db = context;
            StudentService = studentService;
        }

        [HttpGet("GetStudentsByClasses")]
        public IActionResult GetStudentsByClasses(int academicYearId)
        {
            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);
            var schoolId = (int)currentUser.SchoolId;

            var classes = StudentService.GetStudentsByClasses(academicYearId, schoolId);

            return Ok(classes);
        }

        [HttpGet("GetStudents")]
        public IActionResult GetStudents()
        {
            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);
            var schoolId = (int)currentUser.SchoolId;

            var students = StudentService.GetAllStudents(schoolId);

            return Ok(students);
        }

        [HttpPost("CreateStudent")]
        public IActionResult CreateStudent([FromBody]StudentViewModel student)
        {
            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);
            var schoolId = (int)currentUser.SchoolId;

            StudentService.CreateStudent(student);

            return Ok();
        }

    }
}