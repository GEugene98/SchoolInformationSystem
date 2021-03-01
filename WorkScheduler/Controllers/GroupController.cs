using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkScheduler.Models.Enums;
using WorkScheduler.Services.Register;
using WorkScheduler.ViewModels.Register;

namespace WorkScheduler.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class GroupController : Controller
    {
        protected Context Db;
        protected GroupService GroupService;

        public GroupController(Context context, GroupService groupService)
        {
            Db = context;
            GroupService = groupService;
        }

        [HttpGet("GetGroups")]
        public IActionResult GetGroups(int academicYearId, AssociationType type)
        {
            var schoolId = (int)Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name).SchoolId;

            var result = GroupService.GetGroups(academicYearId, schoolId, type);

            return Ok(result);
        }

        [HttpPost("CreateGroup")]
        public IActionResult CreateGroup([FromBody] GroupViewModel group, int academicYearId)
        {
            var schoolId = (int)Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name).SchoolId;

            var result = GroupService.CreateGroup(group, schoolId, academicYearId);

            return Ok(result);
        }

        [HttpPost("UpdateGroup")]
        public IActionResult UpdateGroup([FromBody] GroupViewModel group)
        {
            GroupService.UpdateGroup(group);
            return Ok();
        }

    }
}
