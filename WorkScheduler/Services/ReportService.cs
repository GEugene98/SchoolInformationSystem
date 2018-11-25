using DinkToPdf;
using DinkToPdf.Contracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkScheduler.ViewModels;

namespace WorkScheduler.Services
{
    public class ReportService
    {
        protected CultureInfo culture = CultureInfo.GetCultureInfo("ru-RU");
        protected SchedulerService SchedulerService;
        protected IConverter Converter;

        public ReportService(SchedulerService schedulerService, IConverter converter)
        {
            SchedulerService = schedulerService;
            Converter = converter;
        }

        public byte[] GetGeneralPeriodReport(DateTime start, DateTime end, bool titlePage = false)
        {
            var schedule = SchedulerService.MakeScheduleForPeriod(start, end);

            var html = RenderPeriodReportHTML(schedule);

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

        public byte[] GetGeneralDayReport(DateTime date)
        {
            var schedule = SchedulerService.MakeScheduleForDay(date);

            var html = RenderDayReportHTML(schedule);

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Unit = Unit.Centimeters, Left = 3, Top = 1, Right = 1, Bottom = 1.5 },
                DocumentTitle = $"Отчёт на {date.ToShortDateString()}"
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

        private string RenderPeriodReportHTML(GeneralScheduleViewModel schedule)
        {
            var template = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "assets", "general-template.html"));

            template = template.Replace("%PERIOD%", $"на период с {schedule.Start.ToLongDateString()} по {schedule.End.ToLongDateString()}");
            template = template.Replace("%CONFIRM_DATE%", $"{DateTime.Now.ToLongDateString()}");
            template = template.Replace("%ACCEPT_DATE%", $"{DateTime.Now.ToLongDateString()}");

            Queue<string> formatedDaysToInsertInHTML = new Queue<string>();

            foreach (var day in schedule.Days)
            {
                StringBuilder builder = new StringBuilder();

                string actions = "";

                if (day.Actions != null)
                {
                    actions = string.Join(" ", day.Actions
                    .Select(a =>
                        $@"
                            <div class=""{a.Activity.Color.ToString().ToLower()}"">{a.Name}<br>({a.GetResponsiblesShortNameForms()}, {a.ConfirmationForm.Name.ToLower()})</div><br/>
                        "));
                }

                formatedDaysToInsertInHTML.Enqueue($@"
                            <div>{day.Date.ToShortDateString()} {culture.DateTimeFormat.GetShortestDayName(day.Date.DayOfWeek).ToLower()}</div> 
                            {actions}
                ");
            }

            var rowCount = Math.Ceiling(schedule.Days.Count() / 7.0);

            var tableContent = "";

            for (int i = 0; i < rowCount; i++)
            {
                tableContent += "<tr>";

                for (int j = 0; j < 7; j++)
                {
                    tableContent += "<td>";

                    if (formatedDaysToInsertInHTML.Count != 0)
                        tableContent += formatedDaysToInsertInHTML.Dequeue();

                    tableContent += "</td>";
                }

                tableContent += "</tr>";
            }

            return template.Replace("%TABLE_CONTENT%", tableContent);
        }

        private string RenderDayReportHTML(Day day)
        {
            var template = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "assets", "general-template.html"));

            template = template.Replace("%PERIOD%", $"на {day.Date.ToShortDateString()}");

            var tableContent = "";

            foreach (var action in day.Actions)
            {
                tableContent +=
                    $"<tr class='{action.Activity.Color.ToString().ToLower()}'>" +
                        $"<td>{action.GetResponsiblesShortNameForms()}</td>" +
                        $"<td>{action.Name}</td>" +
                    $"</tr>";
            }

            return template.Replace("%TABLE_CONTENT%", tableContent);
        }

        private string RenderScheduleReportHTML(IEnumerable<ActionViewModel> actions)
        {
            var template = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "assets", "schedule-template.html"));
            template = template.Replace("%CONFIRM_DATE%", $"{DateTime.Now.ToLongDateString()}");
            template = template.Replace("%ACCEPT_DATE%", $"{DateTime.Now.ToLongDateString()}");
            template = template.Replace("%ACTIVITY%", $"{actions.First().Activity.Name.ToLower()}");
            template = template.Replace("%ACADEMIC_YEAR%", $"{actions.First().AcademicYearName}");
            template = template.Replace("%USER%", $"{actions.First().AuthorName}");
            template = template.Replace("%NAME%", $"{actions.First().ScheduleName}");
            
            var tableContent =
                "<th>Дата</th>" +
                "<th>Мероприятие</th>" +
                "<th>Ответственные</th>" +
                "<th>Форма подтверждения выполнения</th>";

            foreach (var action in actions)
            {
                tableContent +=
                    $"<tr>" +
                        $"<td>{action.Date.ToShortDateString()}</td>" +
                        $"<td>{action.Name}</td>" +
                        $"<td>{action.GetResponsiblesShortNameForms()}</td>" +
                        $"<td>{action.ConfirmationForm.Name}</td>" +
                    $"</tr>";
            }

            return template.Replace("%TABLE_CONTENT%", tableContent);
        }

        public byte[] GetScheduleReport(int scheduleId)
        {
            var actions = SchedulerService.GetActionsFor(scheduleId)
                .Where(a => a.Status == Models.Enums.ActionStatus.Accepted);

            if (actions == null || actions.Count() == 0)
            {
                throw new Exception("В плане отсутствуют мероприятия со статусом утверждено");
            }

            var html = RenderScheduleReportHTML(actions);

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
    }
}