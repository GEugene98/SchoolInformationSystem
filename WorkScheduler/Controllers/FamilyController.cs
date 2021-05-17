using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WorkScheduler.Models.Monitoring;
using WorkScheduler.Services.Monitoring;
using WorkScheduler.ViewModels.Monitoring;

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
        public IActionResult Get(int classId)
        {
            var schoolId = (int)Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name).SchoolId;
            var result = FamilyService.Get(schoolId, classId);
            return Ok(result);
        }

        [HttpPut]
        public IActionResult Update([FromBody]Family family)
        {
            FamilyService.Update(family);
            return Ok();

        }

        [HttpPost]
        public IActionResult Create([FromBody]FamilyViewModel family)
        {
            FamilyService.Create(family);
            return Ok();

        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            FamilyService.Delete(id);
            return Ok();

        }


    }
}
   