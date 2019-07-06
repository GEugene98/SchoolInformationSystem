using DinkToPdf;
using DinkToPdf.Contracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkScheduler.Models.Identity;
using WorkScheduler.ViewModels;

namespace WorkScheduler.Services
{
    public class ReportService
    {
        protected CultureInfo culture = CultureInfo.GetCultureInfo("ru-RU");
        protected SchedulerService SchedulerService;
        protected TicketService TicketService;
        protected RenderService RenderService;
        protected IConverter Converter;

        public ReportService(SchedulerService schedulerService, IConverter converter, RenderService renderService, TicketService ticketService)
        {
            SchedulerService = schedulerService;
            TicketService = ticketService;
            RenderService = renderService;
            Converter = converter;
        }

        public byte[] GetGeneralPeriodReport(DateTime start, DateTime end, DateTime confirmDate, DateTime acceptDate, bool titlePage = false)
        {
            var schedule = SchedulerService.MakeScheduleForPeriod(start, end);

            var html = RenderService.GetGeneralPeriodHTML(schedule, confirmDate, acceptDate);

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Landscape,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Unit = Unit.Centimeters, Left = 3, Top = 1, Right = 1, Bottom = 1.5 },
                DocumentTitle = $"Отчёт за период с {start.ToShortDateString()} по {end.ToShortDateString()}"
            };

            var objectSettings = new ObjectSettings
            {
                //PagesCount = true,
                HtmlContent = html,
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "general-report.css") },
                //HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                //FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            return Converter.Convert(pdf);
        }

        public byte[] GetScheduleReport(int scheduleId, DateTime confirmDate, DateTime acceptDate)
        {
            var actions = SchedulerService.GetActionsFor(scheduleId)
                .Where(a => a.Status == Models.Enums.ActionStatus.Accepted);

            if (actions == null || actions.Count() == 0)
            {
                throw new Exception("В плане отсутствуют мероприятия со статусом утверждено");
            }

            var html = RenderService.GetScheduleHTML(actions, confirmDate, acceptDate);

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Unit = Unit.Centimeters, Left = 3, Top = 1, Right = 1, Bottom = 1.5 },
                DocumentTitle = $"План \"{actions.First().ScheduleName}\""
            };

            var objectSettings = new ObjectSettings
            {
                //PagesCount = true,
                HtmlContent = html,
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "general-report.css") },
                //HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                //FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            return Converter.Convert(pdf);

        }

        public byte[] GetTimelineReport(IEnumerable<DateTime> range, User user)
        {
            var ticketPacks = TicketService.GetTickets(range, user);

            var html = RenderService.GetTimelineHTML(ticketPacks).Replace("USERNAME", $"{user.FirstName} {user.SurName}");

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Unit = Unit.Centimeters, Left = 3, Top = 1, Right = 1, Bottom = 1.5 },
                DocumentTitle = $"Отчёт по тайм-листу с {range.First().ToShortDateString()} по {range.Last().ToShortDateString()}"
            };

            var objectSettings = new ObjectSettings
            {
                //PagesCount = true,
                HtmlContent = html,
                WebSettings = { DefaultEncoding = "utf-8" },
                //HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                //FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            return Converter.Convert(pdf);
        }

        //public byte[] GetGeneralDayReport(DateTime date)
        //{
        //    var schedule = SchedulerService.MakeScheduleForDay(date);

        //    var html = RenderDayReportHTML(schedule);

        //    var globalSettings = new GlobalSettings
        //    {
        //        ColorMode = ColorMode.Color,
        //        Orientation = Orientation.Portrait,
        //        PaperSize = PaperKind.A4,
        //        Margins = new MarginSettings { Unit = Unit.Centimeters, Left = 3, Top = 1, Right = 1, Bottom = 1.5 },
        //        DocumentTitle = $"Отчёт на {date.ToShortDateString()}"
        //    };

        //    var objectSettings = new ObjectSettings
        //    {
        //        //PagesCount = true,
        //        HtmlContent = html,
        //        WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "general-report.css") },
        //        //HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
        //        //FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
        //    };

        //    var pdf = new HtmlToPdfDocument()
        //    {
        //        GlobalSettings = globalSettings,
        //        Objects = { objectSettings }
        //    };

        //    return Converter.Convert(pdf);
        //}


        //private string RenderDayReportHTML(Day day)
        //{
        //    var template = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "assets", "general-template.html"));

        //    template = template.Replace("%PERIOD%", $"на {day.Date.ToShortDateString()}");

        //    var tableContent = "";

        //    foreach (var action in day.Actions)
        //    {
        //        tableContent +=
        //            $"<tr class='{action.Activity.Color.ToString().ToLower()}'>" +
        //                $"<td>{action.GetResponsiblesShortNameForms()}</td>" +
        //                $"<td>{action.Name}</td>" +
        //            $"</tr>";
        //    }

        //    return template.Replace("%TABLE_CONTENT%", tableContent);
        //}
    }
}