﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using WorkScheduler.Models;
using WorkScheduler.Models.Identity;
using WorkScheduler.ViewModels;

namespace WorkScheduler.Services
{
    public class NotificationService
    {
        protected Context Db;
        protected UserManager<User> UserManager;
        protected ReportRenderService RenderService;

        public NotificationService(Context context, UserManager<User> userManager, ReportRenderService renderService)
        {
            Db = context;
            UserManager = userManager;
            RenderService = renderService;
        }

        public void SendEmail(string receiverEmail, string subject, string htmlContent, bool serviceMessage = false, IEnumerable<string> fileNames = null)
        {
            if (String.IsNullOrWhiteSpace(receiverEmail))
            {
                return;
            }

            var fromMail = serviceMessage ? "app.service-info@yandex.ru" : "school.service-info@yandex.ru";

            MailAddress from = new MailAddress(fromMail, "Информационная система школы");
            MailAddress to = new MailAddress(receiverEmail);
            MailMessage message = new MailMessage(from, to);

            message.Subject = subject;
            message.Body = htmlContent;
            message.IsBodyHtml = true;

            if (fileNames != null)
                foreach (var attachment in fileNames)
                    message.Attachments.Add(new Attachment(attachment));

            SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 587);

            smtp.Credentials = new NetworkCredential(fromMail, "servicemailpassword1");
            smtp.EnableSsl = true;
            smtp.Send(message);
        }

        public async Task NotifyToConfirmActions(int schoolId)
        {
            var subject = "Мероприятия для согласования";
            var content = $@"
            <h3>Здравствуйте, USERNAME!</h3> 
            <p>Появились новые мероприятия для согласования! Зайдите в программу <a href=""http://school-is.ru/"" target=""_blank"">Планирование</a>, чтобы согласовать или отклонить мероприятия</p>";

            var admins = await UserManager.GetUsersInRoleAsync("Администратор");

            var receivers = admins.Where(r => r.GetNotifications && !String.IsNullOrWhiteSpace(r.Email) && r.SchoolId == schoolId);

            foreach (var user in receivers)
            {
                SendEmail(user.Email, subject, content.Replace("USERNAME", $"{user.FirstName} {user.SurName}"));
            }
        }

        public async Task NotifyToAcceptActions(int schoolId)
        {
            var subject = "Мероприятия для утверждения";
            var content = $@"
            <h3>Здравствуйте, USERNAME!</h3> 
            <p>Появились новые мероприятия для утверждения! Зайдите в программу <a href=""http://school-is.ru/"" target=""_blank"">Планирование</a>, чтобы утвердить или отклонить мероприятия</p>";

            var directors = await UserManager.GetUsersInRoleAsync("Директор");

            var receivers = directors.Where(r => r.GetNotifications && !String.IsNullOrWhiteSpace(r.Email) && r.SchoolId == schoolId);

            foreach (var user in receivers)
            {
                SendEmail(user.Email, subject, content.Replace("USERNAME", $"{user.FirstName} {user.SurName}"));
            }

        }

        public void SendSchedule(WorkSchedule schedule, IEnumerable<ActionViewModel> actions, IEnumerable<User> receivers)
        {
            var subject = $@"План ""{schedule.Name}""";
            var content = $@"
            <h4>Здравствуйте, USERNAME!</h4> 
            <p>План ""{schedule.Name}"" ({schedule.Activity.Name}) по состоянию на {DateTime.Now.ToLongDateString()} {DateTime.Now.ToShortTimeString()}</p>
            <br/> TABLECONTENT";

            var tableContent = @"<table style=""border: 1px solid black; table-layout: fixed;width: 100%;font-size: 12pt;line-height: 1.25;border-collapse: collapse; "" >" +
                @"<th style=""padding: 3px; border: 1px solid black;"">Дата</th>" +
                @"<th style=""padding: 3px; border: 1px solid black;"">Мероприятие</th>" +
                @"<th style=""padding: 3px; border: 1px solid black;"">Ответственные</th>" +
                @"<th style=""padding: 3px; border: 1px solid black;"">Форма подтверждения выполнения</th>" +
                @"<th style=""padding: 3px; border: 1px solid black;"">Статус</th>";

            foreach (var action in actions)
            {
                tableContent +=
                    $@"<tr><td style=""padding: 3px; border: 1px solid black; vertical-align: top;"">{action.Date.ToShortDateString()}</td>" +
                    $@"<td style=""padding: 3px; border: 1px solid black; vertical-align: top;"" >{action.Name}</td>" +
                    $@"<td style=""padding: 3px; border: 1px solid black; vertical-align: top;"">{action.GetResponsiblesShortNameForms()}</td>" +
                    $@"<td style=""padding: 3px; border: 1px solid black; vertical-align: top;"">{action.ConfirmationForm.Name}</td>" +
                    $@"<td style=""padding: 3px; border: 1px solid black; vertical-align: top;"">{action.GetLocalizatedStatus()}</td></tr>";
            }

            tableContent += "</table>";

            content = content.Replace("TABLECONTENT", tableContent);

            foreach (var user in receivers)
            {
                SendEmail(user.Email, subject, content.Replace("USERNAME", $"{user.FirstName} {user.SurName}"));
            }
        }

        public void SendTimeline(List<TicketPackViewModel> ticketPacks, User receiver)
        {
            var subject = $@"Тайм-лист c {ticketPacks.First().Date.ToShortDateString()} по {ticketPacks.Last().Date.ToShortDateString()}";

            var content = RenderService.GetTimelineHTML(ticketPacks);

            SendEmail(receiver.Email, subject, content.Replace("USERNAME", $"{receiver.FirstName} {receiver.SurName}"));
        }

        public void SendGeneralScheduleForPeriod(GeneralScheduleViewModel schedule)
        {

        }

        public void SendProblemReport(string report)
        {
            var recievers = new List<string>(){"stolp-olga@yandex.ru", "geugene1998@ya.ru"};

            foreach(var reciever in recievers)
            {
                SendEmail(reciever, "Отчет пользователя об ошибке", report);
            }
        }
    }
}
