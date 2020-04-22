using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkScheduler.Services;
using WorkScheduler.ViewModels.Scheduler;

namespace WorkScheduler.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ProtocolController : Controller
    {
        protected Context Db;
        protected ProtocolService ProtocolService;
        private readonly Logger Logger;

        public ProtocolController(Context context, ProtocolService protocolService)
        {
            Db = context;
            ProtocolService = protocolService;
            Logger = Logger.GetInstance();
        }

        [HttpGet("MyProtocols")]
        public IActionResult MyProtocols(int year)
        {
            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);
            var result = ProtocolService.GetProtocolList(currentUser.Id, year);
            return Ok(result);
        }

        [HttpGet("AllProtocols")]
        public IActionResult AllProtocols(int year)
        {
            var schoolId = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name).SchoolId;
            var result = ProtocolService.GetFullProtocolList((int)schoolId, year);
            return Ok(result);
        }

        [HttpGet("GetOrCreate")]
        public IActionResult GetOrCreate(int actionId)
        {
            var result = ProtocolService.GetProtocolOrCreate(actionId);
            return Ok(result);
        }

        [HttpGet("Get")]
        public IActionResult GetProtocol(int protocolId)
        {
            var result = ProtocolService.GetProtocol(protocolId);
            return Ok(result);
        }

        [HttpPost("Save")]
        public IActionResult Save([FromBody]ProtocolViewModel protocol)
        {
            ProtocolService.UpdateProtocol(protocol);
            return Ok();
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int protocolId)
        {
            ProtocolService.DeleteProtocol(protocolId);
            return Ok();
        }

        [HttpGet("Exists")]
        public IActionResult Exists(int actionId)
        {
            var result = ProtocolService.ProtocolExists(actionId);
            return Ok(result);
        }

    }
}
