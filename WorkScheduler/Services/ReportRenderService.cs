using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WorkScheduler.ViewModels;
using WorkScheduler.ViewModels.Monitoring;
using WorkScheduler.ViewModels.Register;
using WorkScheduler.ViewModels.Scheduler;
using WorkScheduler.ViewModels.Scheduler.Rendering;

namespace WorkScheduler.Services
{
    public class ReportRenderService
    {
        protected CultureInfo culture = CultureInfo.GetCultureInfo("ru-RU");

        public ReportRenderService()
        {

        }

        public string GetTimelineHTML(List<TicketPackViewModel> ticketPacks)
        {
            var content = $@"
            <p><b>Тайм-лист c {ticketPacks.First().Date.ToShortDateString()} по {ticketPacks.Last().Date.ToShortDateString()} по состоянию на {DateTime.Now.ToLongDateString()} {DateTime.Now.ToShortTimeString()}</b></p>
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

        public string GetProtocolHTML(ProtocolViewModel protocol)
        {
            var template = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "assets", "protocol-template.html"));
            template = template.Replace("%HEADER%", $"{protocol.Header?.Replace("\n", "<br />")}");
            template = template.Replace("%DATE%", $"{protocol.Action.Date.ToLongDateString()}");
            template = template.Replace("%NUMBER%", $"{protocol.Number?.Replace("\n", "<br />")}");
            template = template.Replace("%NAME%", $"{protocol.Name?.Replace("\n", "<br />")}");
            template = template.Replace("%CHAIRMAN%", $"{protocol.Chairman?.Replace("\n", "<br />")}");
            template = template.Replace("%SECRETARY%", $"{protocol.Secretary?.Replace("\n", "<br />")}");
            template = template.Replace("%ATTENDED%", $"{protocol.Attended?.Replace("\n", "<br />")}");

            var parsed = JsonConvert.DeserializeObject<List<Agenda>>(protocol.ProtocolContentJSON);

            var agendaTable = "<br /><table>";

            foreach (var agenda in parsed)
            {
                agendaTable +=
                    $"<tr>" +
                        $"<td style=\"width: 10%; vertical-align: text-top;\">{parsed.IndexOf(agenda) + 1}</td>" +
                        $"<td style=\"width: 60 %\">{agenda.Content}</td>" +
                        $"<td style=\"width: 30%; vertical-align: text-top;\">{agenda.Author?.FullName} <br /> {agenda.Author?.Position}</td>" +
                    $"</tr>";
            }

            agendaTable += "</table><br />";

            var innerContents = "<div>";

            foreach (var agenda in parsed)
            {
                if (agenda?.Listen != null)
                {
                    innerContents +=
                        $"<br />{parsed.IndexOf(agenda) + 1}. СЛУШАЛИ:<br />";

                    foreach (var item in agenda.Listen)
                    {
                        innerContents +=
                            $"{item.User?.FullName} <br /> <p>{item.Content}</p> <br />";
                    }
                }

                if (agenda.Speaked != null)
                {
                    innerContents +=
                        $"ВЫСТУПИЛИ:<br />";

                    foreach (var item in agenda.Speaked)
                    {
                        innerContents +=
                            $"{item.User?.FullName} <br /> <p>{item.Content}</p> <br />";
                    }
                }

                if (agenda.Decided != null)
                {
                    innerContents +=
                        $"РЕШИЛИ (ПОСТАНОВИЛИ):<br /> <p>{agenda.Decided}</p> </div>";
                }

            }

            template = template.Replace("%CONTENT%", agendaTable + innerContents);


            return template;
        }

        public string GetRegisterHTML(AssociationViewModel association,
            GroupViewModel group,
            List<RegisterRow> registerRows,
            List<PlaningRecordViewModel> planingRecords,
            List<FamilyViewModel> families)
        {
            var template = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "assets", "register-template.html"));
            template = template.Replace("%ACADEMIC_YEAR%", association.AcademicYear.Name);
            template = template.Replace("%TEACHER%", association.User.FullName);
            template = template.Replace("%ASSOCIATION%", association.Name);
            template = template.Replace("%GROUP%", group.Name);

            //Рендер таблицы журнала
            if (registerRows.Count > 0 && registerRows[0].Cells.Count > 0)
            {
                var months = new List<MonthInfo>();

                for (var i = 0; i < registerRows[0].Cells.Count; i++)
                {
                    var month = registerRows[0].Cells[i].Date.Value.Month;
                    if (i == 0)
                    {
                        months.Add(new MonthInfo { Month = month, Days = 1, MonthName = monthNames[month - 1] });
                        continue;
                    }
                    var found = months.Where(m => m.Month == month).ToArray();
                    if (found.Length > 0)
                    {
                        found[0].Days++;
                    }
                    else
                    {
                        months.Add(new MonthInfo { Month = month, Days = 1, MonthName = monthNames[month - 1] });
                    }
                }

                var days = registerRows[0].Cells.Select(c => c.Date.Value).ToList();

                var registerHTML = "";

                var skip = 0;

                foreach (var month in months)
                {
                    var monthDays = days.Skip(skip).Take(month.Days);

                    registerHTML +=
                    $@"<table class=""rg-table"">" +
                    $@"<tr class=""head"">" +
                    $@"<td rowspan = ""2"" > Ученики </td>";


                    registerHTML += $@"<td class=""monthname"" colspan=""{month.Days}"">{month.MonthName}</td>";


                    registerHTML += $@"</tr> <tr class=""head"">";

                    foreach (var day in monthDays)
                    {
                        registerHTML += $@"<td>{day.Date.Day}</td>";
                    }

                    registerHTML += $@"</tr>";

                    foreach (var row in registerRows)
                    {
                        registerHTML += $@"<tr>";

                        registerHTML += $@"<td class=""studentname"">{row.Student.FullName}</td>";

                        foreach (var cell in row.Cells.Skip(skip).Take(month.Days))
                        {
                            registerHTML += $@"<td>{cell.Content}</td>";
                        }

                        registerHTML += $@"</tr>";
                    }

                    registerHTML += $@"</table>";
                    skip += month.Days;
                }

                template = template.Replace("%REGISTER%", registerHTML);

                //Рендер КТП

                var ktpHTML = $@"<table class=""rg-table""><thead><tr><th scope=""col"" style=""width:10 %"">Дата</th><th scope = ""col"" style = ""width:70%""> Содержание / Тема </th><th scope = ""col"" style = ""width:10%""> Часы </th><th scope = ""col"" style = ""width:10%""> Примечание </th></tr > </thead > <tbody>";

                foreach (var record in planingRecords)
                {
                    ktpHTML += $@"<tr>
                    <td>{(record.Date.HasValue ? record.Date.Value.ToShortDateString() : "")}</td>
                    <td>{record.Name}</td>
                    <td>{record.Hours}</td>
                    <td>{record.Comment}</td>
                </tr>";
                }

                ktpHTML += $@"</tbody></table>";

                template = template.Replace("%PLANING%", ktpHTML);

                // Рендер информации о семьях

                var familyHTML = $@"<table class=""rg-table""><thead><tr><th scope=""col"">ФИО ребенка</th><th scope=""col"">Дата рождения</th><th scope=""col"">Адрес регистрации</th><th scope=""col"">Адрес проживания</th><th scope=""col"">ФИО матери, телефон, место работы</th><th scope=""col"">ФИО отца, телефон, место работы</th></tr></thead><tbody>";

                foreach (var family in families)
                {
                    familyHTML += $@"<tr>
                    <td>{family.Student.FullName}</td>
                    <td>{(family.Student.Birthday.HasValue ? family.Student.Birthday.Value.ToShortDateString() : "")}</td>
                    <td>{family.RegistrAddres}</td>
                    <td>{family.ResidAddres}</td>
                    <td>{family.FullNameMather + '\n' + family.PhoneMother + '\n' + family.WorkMother}</td>
                    <td>{family.FullNameFather + '\n' + family.PhoneFather + '\n' + family.WorkFather}</td>
                    </tr>";
                }

                familyHTML += $@"</tbody></table>";

                template = template.Replace("%FAMILY%", familyHTML);
            }

            return template;
        }

        private string[] monthNames = new string[] { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };
    }

    class MonthInfo
    {
        public int Month { get; set; }
        public int Days { get; set; }
        public string MonthName { get; set; }
    }
}

