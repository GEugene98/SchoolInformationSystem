using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WorkScheduler.Models.Enums;
using WorkScheduler.Models.Identity;
using WorkScheduler.ViewModels;
using WorkScheduler.ViewModels.Scheduler;

namespace WorkScheduler.Services
{
    public class TicketService
    {
        protected Context Db;
        protected NotificationService NotificationService;

        protected CultureInfo culture = CultureInfo.GetCultureInfo("ru-RU");

        public TicketService(Context context, NotificationService notificationService)
        {
            Db = context;
            NotificationService = notificationService;
        }

        public List<TicketPackViewModel> GetTickets(IEnumerable<DateTime> range, User user)
        {
            var dateFrom = range.ToArray()[0].AddHours(3);
            var dateTo = range.ToArray()[1].AddHours(3);

            var actionUsers = Db.ActionUsers
                .Include(au => au.User)
                .Include(au => au.Action)
                .ToList();
            var tickets = Db.Tickets
                .Include(t => t.Action)
                .Include(t => t.Action.ConfirmationForm)
                .Include(t => t.Action.WorkSchedule)
                .Include(t => t.Action.WorkSchedule.Activity)
                .Where(t => t.UserId == user.Id && t.Date.HasValue && (!t.Status.HasValue || t.Status == TicketStatus.Accepted || t.Status == TicketStatus.Done));

            if (dateTo == null || dateTo.ToShortDateString() == "01.01.0001")
            {
                dateTo = DateTime.Now;
            }

            if (dateFrom == null || dateFrom.ToShortDateString() == "01.01.0001")
            {
                dateFrom = DateTime.Now;
            }

            var groupedTickets = tickets
                .Where(t => t.Date.Value.Date >= dateFrom.Date && t.Date.Value.Date <= dateTo.Date)
                .ToList()
                .OrderBy(t => t.Date.Value.Date)
                .ThenBy(t => t.Hours)
                .ThenBy(t => t.Minutes)
                .GroupBy(t => t.Date.Value.Date);

            var ticketPacks = new List<TicketPackViewModel>();


            for (DateTime i = dateFrom.Date; i <= dateTo.Date; i = i.AddDays(1))
            {
                var group = groupedTickets.FirstOrDefault(g => g.Key.Date == i.Date);

                if (group == null)
                {
                    var tGroups = new List<TicketTimeGroup>();

                    for (var j = 8; j <= 17; j++)
                    {
                        var timeGroup = new TicketTimeGroup
                        {
                            Hour = j,
                        };

                        tGroups.Add(timeGroup);
                    }

                    var mock = new TicketPackViewModel
                    {
                        Date = i,
                        DateToShow = FirstUpper(culture.DateTimeFormat.GetDayName(i.DayOfWeek)) + "  " + i.ToLongDateString(),
                        TimeGroups = tGroups
                    };

                    ticketPacks.Add(mock);

                    continue;
                }


                var ticketsToPack = group.Select(t => new TicketViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Date = t.Date.Value,
                    Comment = t.Comment,
                    Done = t.Done,
                    Important = t.Important,
                    HasChecklist = t.ChecklistId != null,
                    IsExpiered = t.Date.HasValue && DateTime.Now.Date > t.Date.Value.Date && t.Status != TicketStatus.Done,
                    User = new UserViewModel
                    {
                        Id = t.User.Id,
                        Name = t.User.UserName
                    },
                    Action = t.Action != null ? new ActionViewModel
                    {
                        Id = t.Action.Id,
                        Name = t.Action.Name,
                        Activity = new ActivityViewModel
                        {
                            Id = t.Action.WorkSchedule.Activity.Id,
                            Name = t.Action.WorkSchedule.Activity.Name,
                            Color = t.Action.WorkSchedule.Activity.Color
                        },
                        IsDeleted = t.Action.IsDeleted,
                        ConfirmationForm = new ConfirmationFormViewModel
                        {
                            Id = t.Action.ConfirmationForm.Id,
                            Name = t.Action.ConfirmationForm.Name
                        },
                        Status = t.Action.Status,
                        Date = t.Action.Date.AddHours(3),
                        Responsibles = actionUsers
                    .Where(ar => ar.ActionId == t.Action.Id)
                    .Select(ar => new UserViewModel
                    {
                        Id = ar.User.Id,
                        Name = ar.User.UserName,
                        FirstName = ar.User.FirstName,
                        LastName = ar.User.LastName,
                        SurName = ar.User.SurName,
                    })
                    .ToList(),
                    } : null,
                    Hours = t.Hours,
                    Minutes = t.Minutes ?? 0,
                    AutoGenerated = t.AutoGenerated
                });

                var timeGroups = new List<TicketTimeGroup>();
                var minTicketHour = ticketsToPack.Select(ttp => ttp.Hours).Min().Value;
                var startHour = minTicketHour < 8 ? minTicketHour : 8;
                var maxTicketHour = ticketsToPack.Select(ttp => ttp.Hours).Max().Value;
                var endHour = maxTicketHour > 17 ? maxTicketHour : 17;

                for (var j = startHour; j <= endHour; j++)
                {
                    var timeGroup = new TicketTimeGroup
                    {
                        Hour = j,
                        Tickets = ticketsToPack.Where(t => t.Hours == j).ToList()
                    };

                    timeGroups.Add(timeGroup);
                }

                var pack = new TicketPackViewModel
                {
                    Date = group.Key,
                    DateToShow = FirstUpper(culture.DateTimeFormat.GetDayName(i.DayOfWeek)) + "  " +  group.Key.ToLongDateString(),
                    TimeGroups = timeGroups
                };

                ticketPacks.Add(pack);
            }


            return ticketPacks;
        }

        public IEnumerable<TicketViewModel> GetAssignedTickets(string userId)
        {
            var tickets = Db.Tickets
                .Include(t => t.Checklist)
                .Include(t => t.Checklist.User)
                .Where(t => t.UserId == userId && t.ChecklistId != null && t.Status == TicketStatus.Assigned)
                .ToList()
                .Select(t => new TicketViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    AssignmentComment = t.AssignmentComment,
                    Important = t.Important,
                    Date = t.Date,
                    Hours = t.Hours,
                    Minutes = t.Minutes,
                    Checklist = new ChecklistViewModel
                    {
                        Id = t.Checklist.Id,
                        Name = t.Checklist.Name,
                        User = new UserViewModel
                        {
                            Id = t.Checklist.User.Id,
                            Name = t.Checklist.User.UserName,
                            FullName =  $"{t.Checklist.User.LastName} {t.Checklist.User.FirstName[0]}. {t.Checklist.User.SurName[0]}."
                        },
                        Deadline = t.Checklist.Deadline,
                        Comment = t.Checklist.Comment,
                        CreatedOn = t.Checklist.CreatedOn
                    }
                });

            return tickets;
        }

        public int GetAssignedTicketCount(string userId)
        {
            return Db.Tickets.Where(t => t.UserId == userId && t.ChecklistId != null && t.Status == TicketStatus.Assigned).Count();
        }

        public void AcceptTicket(long ticketId, DateTime? date, byte? hours, byte? minutes)
        {
            if (!date.HasValue || !hours.HasValue || !minutes.HasValue)
            {
                throw new Exception("Не указана дата или время");
            }

            var ticket = Db.Tickets.FirstOrDefault(t => t.Id == ticketId);

            if (ticket == null)
            {
                throw new Exception("Запись не найдена");
            }
  
            ticket.Date = date;

            if (ticket.Hours != hours || ticket.Minutes != minutes)
            {
                ticket.Minutes = minutes;
                ticket.Hours = hours;
            }

            ticket.Status = TicketStatus.Accepted;

            Db.SaveChanges();
        }

        string FirstUpper(string str)
        {
            string[] s = str.Split(' ');

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i].Length > 1)
                    s[i] = s[i].Substring(0, 1).ToUpper() + s[i].Substring(1, s[i].Length - 1).ToLower();
                else s[i] = s[i].ToUpper();
            }
            return string.Join(" ", s);
        }
    }
}
