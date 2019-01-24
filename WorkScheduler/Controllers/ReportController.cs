using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkScheduler.Services;

namespace WorkScheduler.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class ReportController : Controller
    {
        protected ReportService ReportService;
        private readonly Logger Logger;

        public ReportController(ReportService reportService)
        {
            ReportService = reportService;
            Logger = Logger.GetInstance();
        }

        [HttpGet("ForPeriod")]
        public IActionResult ForPeriod(int startDay, int startMonth, int startYear, int endDay, int endMonth, int endYear)
        {
            var start = new DateTime(startYear, startMonth, startDay);
            var end = new DateTime(endYear, endMonth, endDay);

            var report = ReportService.GetGeneralPeriodReport(start, end);
            return File(report, "application/pdf");
        }

        [HttpGet("ForDay")]
        public IActionResult ForDay(int day, int month, int year)
        {
            var date = new DateTime(year, month, day);

            var report = ReportService.GetGeneralDayReport(date);
            return File(report, "application/pdf");
        }

        [HttpGet("ForSchedule")]
        public IActionResult ForSchedule(int scheduleId)
        {
            try
            {
                var report = ReportService.GetScheduleReport(scheduleId);
                return File(report, "application/pdf");
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                return BadRequest(ex.Message);
            }
        }
    }
}