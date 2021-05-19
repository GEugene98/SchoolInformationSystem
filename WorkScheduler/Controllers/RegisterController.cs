using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkScheduler.Services.Register;

namespace WorkScheduler.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class RegisterController : Controller
    {
        protected Context Db;
        protected RegisterService RegisterService;

        public RegisterController(Context context, RegisterService registerService)
        {
            Db = context;
            RegisterService = registerService;
        }

        [HttpGet]
        [Route("GetRecords")]
        public IActionResult GetRecords(int academicYearId, int associationId, int groupId)
        {
            //var schoolId = (int)Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name).SchoolId;
            var result = RegisterService.GetRecords(academicYearId, associationId, groupId);
            return Ok(result);
        }

        [HttpGet]
        [Route("MakeMark")]
        public IActionResult MakeMark(int planingRecordId, int studentId, string content, int? cellId)
        {
            RegisterService.MakeMark(studentId, planingRecordId, content, cellId);
            return Ok();
        }
    }


}
