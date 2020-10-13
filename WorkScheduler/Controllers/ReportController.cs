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
    public class ReportController : Controller
    {
        protected Context Db;
        protected ReportService ReportService;
        private readonly Logger Logger;

        public ReportController(Context context, ReportService reportService)
        {
            Db = context;
            ReportService = reportService;
            Logger = Logger.GetInstance();
        }

        [HttpGet("ForPeriod")]
        public IActionResult ForPeriod(int startDay, int startMonth, int startYear, int endDay, int endMonth, int endYear, int confDay, int confMonth, int confYear, int acpDay, int acpMonth, int acpYear)
        {
            var start = new DateTime(startYear, startMonth, startDay);
            var end = new DateTime(endYear, endMonth, endDay);
            var confDate = new DateTime(confYear, confMonth, confDay);
            var acpDate = new DateTime(acpYear, acpMonth, acpDay);

            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);

            var report = ReportService.GetGeneralPeriodReport(start, end, confDate, acpDate, currentUser);
            return File(report, "application/pdf");
        }

        //[HttpGet("ForDay")]
        //public IActionResult ForDay(int day, int month, int year)
        //{
        //    //var date = new DateTime(year, month, day);

        //    //var report = ReportService.GetGeneralDayReport(date);
        //    //return File(report, "application/pdf");
        //}

        [HttpGet("ForSchedule")]
        public IActionResult ForSchedule(int scheduleId, int confDay, int confMonth, int confYear, int acpDay, int acpMonth, int acpYear)
        {
            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);

            var confDate = new DateTime(confYear, confMonth, confDay);
            var acpDate = new DateTime(acpYear, acpMonth, acpDay);

            try
            {
                var report = ReportService.GetScheduleReport(scheduleId, confDate, acpDate, currentUser);
                return File(report, "application/pdf");
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ForTimeline")]
        public IActionResult ForTimeline(int startDay, int startMonth, int startYear, int endDay, int endMonth, int endYear)
        {
            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);

            var range = new List<DateTime>();
            range.Add(new DateTime(startYear, startMonth, startDay));
            range.Add(new DateTime(endYear, endMonth, endDay));

            try
            {
                var report = ReportService.GetTimelineReport(range, currentUser);
                return File(report, "application/pdf");
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Protocol")]
        public IActionResult Protocol(int protocolId)
        {
            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);

            try
            {
                var report = ReportService.GetProtocolReport(protocolId, (int)currentUser.SchoolId);
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