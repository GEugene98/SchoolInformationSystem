﻿using System;
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
    public class ClassController : Controller
    {
        protected Context Db;
        protected ClassService ClassService;
        protected StudentService StudentService;

        public ClassController(Context context, ClassService classService, StudentService studentService)
        {
            Db = context;
            ClassService = classService;
            StudentService = studentService;
        }

        [HttpPost("CreateClass")]
        public IActionResult CreateClass([FromBody]ClassVievModel classModel)
        {
            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);
            var schoolId = (int)currentUser.SchoolId;

            var classId = ClassService.CreateClass(classModel.AcademicYearId, schoolId, classModel.Name);

            if (classModel.Students != null && classModel.Students.Count() > 0)
            {
                StudentService.PutStudentsToClass(classModel.Students.Select(s => s.Id), classId, classModel.AcademicYearId);
            }

            return Ok();
        }

        [HttpDelete("DeleteClass")]
        public IActionResult DeleteClass(int id)
        {
            ClassService.DeleteClass(id);
            return Ok();
        }
    }
}
