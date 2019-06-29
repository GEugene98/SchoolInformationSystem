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
    public class RenderService
    {
        protected CultureInfo culture = CultureInfo.GetCultureInfo("ru-RU");

        public RenderService()
        {

        }

        public string GetTimelineHTML(List<TicketPackViewModel> ticketPacks)
        {
            var content = $@"
            <p><b>Органайзер c {ticketPacks.First().Date.ToShortDateString()} по {ticketPacks.Last().Date.ToShortDateString()} по состоянию на {DateTime.Now.ToLongDateString()} {DateTime.Now.ToShortTimeString()}</b></p>
            <br/> TABLECONTENT";

            var tableStart = @"<table style=""font-size: 14px; background: white; max-width: 70%; width: 100%; border-collapse: collapse; text-align: left; "" >";
            var th = @"<th style=""font-weight: normal; color: #039;  border-bottom: 2px solid #6678b1; padding: 10px 8px;""> %DATA% </th>";
            var td = @"<td style=""color: #669; padding: 9px 8px; transition: .3s linear; border-bottom: 1px solid #ccc;""> %DATA% </th>";

            var tableContent = new StringBuilder();

            foreach (var pack in ticketPacks)
            {
                tableContent.Append($@"<div style=""display:block; page-break-inside:avoid;""><h3>{pack.DateToShow}</h3>");
                tableContent.Append(tableStart);
                tableContent.Append("<thead>");
                tableContent.Append("<tr>");
                tableContent.Append(th.Replace("%DATA%", "Время"));
                tableContent.Append(th.Replace("%DATA%", "Наименование"));
                tableContent.Append(th.Replace("%DATA%", "Мероприятие"));
                tableContent.Append(th.Replace("%DATA%", "Комментарий"));
                tableContent.Append("</tr>");
                tableContent.Append("</thead>");

                foreach (var timeGroup in pack.TimeGroups)
                {
                    tableContent.Append("<tbody>");

                    tableContent.Append("<tr>");
                    tableContent.Append(td.Replace("%DATA%", $"<b>{timeGroup.Hour}:00</b>"));
                    tableContent.Append(td.Replace("%DATA%", ""));
                    tableContent.Append(td.Replace("%DATA%", ""));
                    tableContent.Append(td.Replace("%DATA%", ""));
                    tableContent.Append("</tr>");

                    if (timeGroup.Tickets != null)
                    {
                        foreach (var ticket in timeGroup.Tickets)
                        {
                            var trStyle = "";
                            trStyle += ticket.Important ? "font-weight: bolder !important; " : "";
                            trStyle += ticket.Done ? "color: darkgrey !important; " : "";

                            tableContent.Append($@"<tr style=""{trStyle}"">");
                            var minutes = ticket.Minutes.Value < 10 ? $"0{ticket.Minutes.Value}" : ticket.Minutes.Value.ToString();
                            var time = ticket.Minutes.Value == 0 ? " " : $"{ticket.Hours.Value}:{minutes}";
                            tableContent.Append(td.Replace("%DATA%", $"{time}"));
                            tableContent.Append(td.Replace("%DATA%", $"{ticket.Name}"));
                            var action = ticket.Action == null ? " " : $"&#171;{ticket.Action?.Name}&#187; <br />  {ticket.Action?.GetResponsiblesShortNameForms()} <br />ФП: {ticket.Action?.ConfirmationForm.Name}";
                            tableContent.Append(td.Replace("%DATA%", action));
                            tableContent.Append(td.Replace("%DATA%", $"{ticket.Comment}"));
                            tableContent.Append("</tr>");
                        }
                    }

                    tableContent.Append("</tbody>");
                }

                tableContent.Append("</table></div><br /><br />");
            }

            return content.Replace("TABLECONTENT", tableContent.ToString());
        }

        public string GetGeneralPeriodHTML(GeneralScheduleViewModel schedule, DateTime confirmDate, DateTime acceptDate)
        {
            var template = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "assets", "general-template.html"));

            template = template.Replace("%PERIOD%", $"на период с {schedule.Start.ToLongDateString()} по {schedule.End.ToLongDateString()}");
            template = template.Replace("%CONFIRM_DATE%", $"{confirmDate.ToLongDateString()}");
            template = template.Replace("%ACCEPT_DATE%", $"{acceptDate.ToLongDateString()}");

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

        public string GetScheduleHTML(IEnumerable<ActionViewModel> actions, DateTime confirmDate, DateTime acceptDate)
        {
            var template = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "assets", "schedule-template.html"));
            template = template.Replace("%CONFIRM_DATE%", $"{confirmDate.ToLongDateString()}");
            template = template.Replace("%ACCEPT_DATE%", $"{acceptDate.ToLongDateString()}");
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
    }
}
