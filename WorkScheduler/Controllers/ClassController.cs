using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WorkScheduler.Services.Monitoring;
using WorkScheduler.ViewModels.Monitoring.Shared;

namespace WorkScheduler.Controllers
{
    public class ClassController : Controller
    {
        protected Context Db;
        protected ClassService ClassService;

        public ClassController(Context context, ClassService classService)
        {
            Db = context;
            ClassService = classService;
        }

        [HttpGet("GetClasses")]
        public IActionResult GetClasses(int academicYearId)
        {
            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);
            var schoolId = (int)currentUser.SchoolId;

            var classes = ClassService.GetStudentsByClasses(academicYearId, schoolId);

            return Ok(classes);
        }

        [HttpPost("CreateClass")]
        public IActionResult CreateClass([FromBody]ClassVievModel classModel)
        {
            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);
            var schoolId = (int)currentUser.SchoolId;

            

            return Ok(classes);
        }
    }
}
