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
    public class AssociationController : Controller
    {
        protected Context Db;
        protected AssociationService AssociationService;

        public AssociationController(Context context, AssociationService associationService)
        {
            Db = context;
            AssociationService = associationService;
        }

        [HttpGet("GetAssociations")]
        public IActionResult GetAssociations(AssociationType type, int academicYearId)
        {
            var schoolId = (int)Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name).SchoolId;

            var result = AssociationService.GetAssociations(type, schoolId, academicYearId);

            return Ok(result);
        }

        [HttpDelete("DeleteAssociation")]
        public IActionResult DeleteAssociation(int id)
        {
            AssociationService.DeleteAssociation(id);
            return Ok();
        }

        [HttpPost("CreateAssociation")]
        public IActionResult CreateAssociation([FromBody]AssociationViewModel association, int academicYearId)
        {
            var schoolId = (int)Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name).SchoolId;

            var result = AssociationService.CreateAssociation(association, schoolId, academicYearId);

            return Ok(result);
        }

        [HttpPost("EditAssociation")]
        public IActionResult EditAssociation([FromBody] AssociationViewModel association)
        {
            AssociationService.EditAssotiation(association);
            return Ok();
        }
    }
}
