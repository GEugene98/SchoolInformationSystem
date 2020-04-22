using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkScheduler.Services;

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


    }
}
