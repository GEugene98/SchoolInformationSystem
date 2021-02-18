using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkScheduler.Services.Register;

namespace WorkScheduler.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class PlaningRecordController : Controller
    {
        protected Context Db;
        protected PlaningRecordService PlaningRecordService;

        public PlaningRecordController(Context context, PlaningRecordService planingRecordService)
        {
            Db = context;
            PlaningRecordService = planingRecordService;
        }

        [HttpPost]
        [Route("UploadPlaningExcel")]
        public IActionResult UploadPlaningExcel()
        {
            IFormFile file;

            if (HttpContext.Request.Form.Files.Count > 0)
            {
                file = HttpContext.Request.Form.Files[0];
            }
            else return BadRequest();


            PlaningRecordService.ParseExcelAndWriteToDB(file);

            return Ok();
        }
    }
}
