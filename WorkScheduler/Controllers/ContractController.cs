using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkScheduler.Services.Monitoring;
using WorkScheduler.ViewModels.Monitoring;

namespace WorkScheduler.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ContractController : Controller
    {
        protected Context Db;
        protected ContractService ContractService;

        public ContractController(Context context, ContractService contractService)
        {
            ContractService = contractService;
            Db = context;
        }

        [HttpGet("GetContracts")]
        public IActionResult GetContracts()
        {
            var schoolId = (int)Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name).SchoolId;
            var result = ContractService.GetContracts(schoolId);
            return Ok(result);
        }

        [HttpPost("CreateContract")]
        public IActionResult CreateContract([FromBody] ContractViewModel contract)
        {
            var schoolId = (int)Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name).SchoolId;
            contract.SchoolId = schoolId;
            var result = ContractService.CreateContract(contract);
            return Ok(result);
        }

        [HttpPost("UpdateContract")]
        public IActionResult UpdateContract([FromBody] ContractViewModel contract)
        {
            ContractService.UpdateContract(contract);
            return Ok();
        }

        [HttpDelete("DeleteContract")]
        public IActionResult DeleteContract(long id)
        {
            ContractService.DeleteContract(id);
            return Ok();
        }
    }
}
