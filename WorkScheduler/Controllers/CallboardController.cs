using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkScheduler.Services;
using WorkScheduler.ViewModels;

namespace WorkScheduler.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class CallboardController : Controller
    {
        CallboardService CallboardService;
        Context Db;

        public CallboardController(Context context, CallboardService callboardService)
        {
            CallboardService = callboardService;
            Db = context;
        }

        [HttpGet("GetAvailablePosts")]
        public IActionResult GetAvailablePosts()
        {
            var schoolId = (int)Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name).SchoolId;

            var result = CallboardService.GetPosts(schoolId);

            return Ok(result);
        }

        [HttpPost("SendPost")]
        public IActionResult SendPost([FromBody]PostViewModel post)
        {
            var userId = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name).Id;

            CallboardService.PublishPost(post, userId);

            return Ok();
        }

        [HttpDelete("DeletePost")]
        public IActionResult DeletePost(long id)
        {
            CallboardService.DeletePost(id);

            return Ok();
        }
    }
}
