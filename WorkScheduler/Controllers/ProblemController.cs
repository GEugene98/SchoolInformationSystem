using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkScheduler.Services;

namespace WorkScheduler.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ProblemController : Controller
    {
        protected Context Db;
        protected NotificationService NotificationService;

        public ProblemController(Context context, NotificationService notificationService)
        {
            Db = context;
            NotificationService = notificationService;
        }

        [HttpGet("Report")]
        public IActionResult Report(string report)
        {
            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);

            var message = "Пользователь " + currentUser.LastName + " " + currentUser.School.Name + " " + currentUser.Email + " " + currentUser.FirstName + " " + currentUser.SurName + " сообщает: <br/>" + report;

            NotificationService.SendProblemReport(message);

            return Ok();
        }
    }
}