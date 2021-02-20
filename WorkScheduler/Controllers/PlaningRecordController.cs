using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WorkScheduler.Services.Register;
using WorkScheduler.ViewModels.Register;

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

        [HttpGet]
        [Route("GetRecords")]
        public IActionResult GetRecords(int academicYearId, int associationId, int groupId)
        {
            var result = PlaningRecordService.GetRecords(academicYearId, associationId, groupId);
            return Ok(result);
        }

        [HttpPost]
        [Route("UpdateRecord")]
        public IActionResult UpdateRecord([FromBody]PlaningRecordViewModel record)
        {
            PlaningRecordService.UpdateRecord(record);
            return Ok();
        }

        [HttpDelete]
        [Route("DeleteRecord")]
        public IActionResult DeleteRecord(long recordId)
        {
            PlaningRecordService.DeleteRecord(recordId);
            return Ok();
        }

        [HttpPost]
        [Route("UploadPlaningExcel")]
        public IActionResult UploadPlaningExcel(string importModelJSON)
        {
            IFormFile file;

            var importModel = JsonConvert.DeserializeObject<ImportPlaning>(importModelJSON);

            if (HttpContext.Request.Form.Files.Count > 0)
            {
                file = HttpContext.Request.Form.Files[0];
            }
            else return BadRequest();

            try
            {
                PlaningRecordService.ParseExcelAndWriteToDB(file, importModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}
