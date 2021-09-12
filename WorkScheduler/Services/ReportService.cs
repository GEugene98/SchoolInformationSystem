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
using WorkScheduler.Services.Monitoring;
using WorkScheduler.Services.Register;
using WorkScheduler.ViewModels;
using WorkScheduler.ViewModels.Register;

namespace WorkScheduler.Services
{
    public class ReportService
    {
        protected CultureInfo culture = CultureInfo.GetCultureInfo("ru-RU");
        protected SchedulerService SchedulerService;
        protected TicketService TicketService;
        protected ProtocolService ProtocolService;
        protected ReportRenderService RenderService;
        protected IConverter Converter;

        protected AssociationService AssociationService;
        protected GroupService GroupService;
        protected RegisterService RegisterService;
        protected PlaningRecordService PlaningRecordService;
        protected FamilyService FamilyService;

        public ReportService(SchedulerService schedulerService,
            IConverter converter,
            ReportRenderService renderService,
            TicketService ticketService,
            ProtocolService protocolService,
            AssociationService associationService,
            GroupService groupService,
            RegisterService registerService,
            PlaningRecordService planingRecordService,
            FamilyService familyService
            )
        {
            SchedulerService = schedulerService;
            TicketService = ticketService;
            ProtocolService = protocolService;
            RenderService = renderService;
            Converter = converter;

            AssociationService = associationService;
            GroupService = groupService;
            RegisterService = registerService;
            PlaningRecordService = planingRecordService;
            FamilyService = familyService;
        }

        public byte[] GetGeneralPeriodReport(DateTime start, DateTime end, DateTime confirmDate, DateTime acceptDate, User user, bool titlePage = false)
        {
            var schedule = SchedulerService.MakeScheduleForPeriod(start, end, user);

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

        public byte[] GetScheduleReport(int scheduleId, DateTime confirmDate, DateTime acceptDate, User user)
        {
            var actions = SchedulerService.GetActionsFor(scheduleId, user)
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
                //PagesCount = true,0
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

        public byte[] GetProtocolReport(int protcolId, int schoolId)
        {
            var protocol = ProtocolService.GetProtocol(protcolId, schoolId);

            var html = RenderService.GetProtocolHTML(protocol);

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Grayscale,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Unit = Unit.Centimeters, Left = 3, Top = 1, Right = 1, Bottom = 1.5 },
                DocumentTitle = $"Протокол заседания {protocol.Name}"
            };

            var objectSettings = new ObjectSettings
            {
                //PagesCount = true,0
                HtmlContent = html,
                WebSettings = { DefaultEncoding = "utf-8" },
                //HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Times New Roman", FontSize = 10, Line = false, Center = "[page]" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            return Converter.Convert(pdf);
        }

        public byte[] GetRegisterReport(int academicYearId, int assotiationId, int groupId)
        {
            var records = RegisterService.GetRecords(academicYearId, assotiationId, groupId).ToList();
            var planings = PlaningRecordService.GetRecords(academicYearId, assotiationId, groupId).ToList();
            var association = AssociationService.Get(assotiationId);
            var group = GroupService.Get(groupId);
            var families = FamilyService.GetByGroup(groupId);
            var html = RenderService.GetRegisterHTML(association, group, records, planings, families);

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Grayscale,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Unit = Unit.Centimeters, Left = 2, Top = 1, Right = 2, Bottom = 1.5 },
                DocumentTitle = $"Журнал {association.Name}",
                DPI = 380
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = html,
                WebSettings = { DefaultEncoding = "utf-8" },
                //HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Times New Roman", FontSize = 10, Line = false, Center = "[page]" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            return Converter.Convert(pdf);
        }
    }
}