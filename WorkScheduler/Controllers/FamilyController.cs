using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WorkScheduler.Models.Monitoring;
using WorkScheduler.Services.Monitoring;

namespace WorkScheduler.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class FamilyController : Controller
    {
        public FamilyService FamilyService { get; set; }
        public Context Db { get; set; }

        public FamilyController(FamilyService familyService, Context context)
        {
            FamilyService = familyService;
            Db = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var schoolId = (int)Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name).SchoolId;
            var result = FamilyService.Get(schoolId);
            return Ok(result);
        }

        [HttpPut]
        public IActionResult Update(Family family)
        {
            return Ok();

        }

        [HttpPost("[action]")]
        public IActionResult Create(Family family)
        {

            return Ok();

        }

        [HttpDelete("[action]")]
        public IActionResult Delete(int id)
        {
            return Ok();

        }


    }
}
   