using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkScheduler.ViewModels;
using WorkScheduler.Models;
using System.Globalization;
using WorkScheduler.Models.Enums;
using WorkScheduler.Models.Identity;

namespace WorkScheduler.Services
{
    public class SchedulerService
    {
        protected Context Db;
        protected NotificationService NotificationService;

        protected CultureInfo culture = CultureInfo.GetCultureInfo("ru-RU");

        public SchedulerService(Context context, NotificationService notificationService)
        {
            Db = context;
            NotificationService = notificationService;
        }

        public WorkSchedule GetSchedule(int scheduleId)
        {
            return Db.WorkSchedules
                .Include(s => s.Actions)
                .Include(s => s.AcademicYear)
                .Include(s => s.Activity)
                .FirstOrDefault(s => s.Id == scheduleId);
        }

        public IEnumerable<WorkScheduleViewModel> GetWorkSchedules(string userId)
        {
            return Db.WorkSchedules
                .Where(ws => ws.UserId == userId)
                .Select(ws => new WorkScheduleViewModel
                {
                    Id = ws.Id,
                    Name = ws.Name,
                    AcademicYear = new AcademicYearViewModel
                    {
                        Id = ws.AcademicYear.Id,
                        Name = ws.AcademicYear.Name
                    },
                    Activity = new ActivityViewModel
                    {
                        Id = ws.Activity.Id,
                        Name = ws.Activity.Name,
                        Color = ws.Activity.Color
                    },
                    User = new UserViewModel
                    {
                        Id = ws.User.Id,
                        Name = ws.User.UserName,
                        FirstName = ws.User.FirstName,
                        LastName = ws.User.LastName,
                        SurName = ws.User.SurName
                    }
                })
                .ToList();
        }

        public IEnumerable<WorkScheduleViewModel> GetFullWorkSchedules(string userId)
        {
            return Db.WorkSchedules
                .Where(ws => ws.UserId == userId)
                .Select(ws => new WorkScheduleViewModel
                {
                    Id = ws.Id,
                    Name = ws.Name,
                    AcademicYear = new AcademicYearViewModel
                    {
                        Id = ws.AcademicYear.Id,
                        Name = ws.AcademicYear.Name
                    },
                    Activity = new ActivityViewModel
                    {
                        Id = ws.Activity.Id,
                        Name = ws.Activity.Name,
                        Color = ws.Activity.Color
                    },
                    User = new UserViewModel
                    {
                        Id = ws.User.Id,
                        Name = ws.User.UserName,
                        FirstName = ws.User.FirstName,
                        LastName = ws.User.LastName,
                        SurName = ws.User.SurName
                    }
                })
                .ToList();
        }

        public IEnumerable<ActionViewModel> GetActionsFor(int workScheduleId, int count = 100000)
        {
            var actionUsers = Db.ActionUsers
                .Include(au => au.User)
                .Include(au => au.Action)
                .ToList();

            var workSchedule = Db.WorkSchedules
                .Include(ws => ws.AcademicYear)
                .Include(ws => ws.Activity)
                .FirstOrDefault(ws => ws.Id == workScheduleId);

            return Db.Actions
                .Include(a => a.ConfirmationForm)
                .Where(a => a.WorkScheduleId == workScheduleId && !a.IsDeleted)
                .OrderBy(a => a.Date)
                .Take(count)
                .ToList()
                .Select(a => new ActionViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Date = a.Date,
                    Status = a.Status,
                    ConfirmationForm = new ConfirmationFormViewModel
                    {
                        Id = a.ConfirmationForm.Id,
                        Name = a.ConfirmationForm.Name
                    },
                    Responsibles = actionUsers
                    .Where(ar => ar.ActionId == a.Id)
                    .Select(ar => new UserViewModel
                    {
                        Id = ar.User.Id,
                        Name = ar.User.UserName,
                        FirstName = ar.User.FirstName,
                        LastName = ar.User.LastName,
                        SurName = ar.User.SurName,
                    })
                    .ToList(),
                    ScheduleName = workSchedule.Name,
                    Activity = new ActivityViewModel
                    {
                        Id = workSchedule.Activity.Id,
                        Name = workSchedule.Activity.Name
                    },
                    AcademicYearName = workSchedule.AcademicYear.Name,
                    AuthorName = $"{workSchedule.User.LastName} {workSchedule.User.FirstName[0]}. {workSchedule.User.SurName[0]}."
                });
        }

        public void AddWorkSchedule(string userId, int activityId, int academicYearId, string name)
        {
            if (String.IsNullOrWhiteSpace(userId))
            {
                throw new Exception("Ошибка авторизации. Попробуйте выполнить вход в приложение заново");
            }

            //var foundSchedule = Db.WorkSchedules.FirstOrDefault(s => s.UserId == userId && s.ActivityId == activityId && s.AcademicYearId == academicYearId);

            //if (foundSchedule != null)
            //{
            //    throw new Exception("В указанном учебном году у вас уже создан план по указанному направлению деятельности.");
            //}

            if (String.IsNullOrWhiteSpace(name) || name == "undefined")
            {
                throw new Exception("Введите имя плана");
            }

            if (activityId == 0)
            {
                throw new Exception("Укажите направление деятельности");
            }

            if (academicYearId == 0)
            {
                throw new Exception("Укажите учебный год");
            }

            var workSchedule = new WorkSchedule
            {
                UserId = userId,
                Name = name,
                ActivityId = activityId,
                AcademicYearId = academicYearId,
            };

            Db.WorkSchedules.Add(workSchedule);
            Db.SaveChanges();
        }

        public void DeleteSchedule(int scheduleId)
        {
            var schedule = Db.WorkSchedules.FirstOrDefault(ws => ws.Id == scheduleId);

            if (schedule == null)
            {
                throw new Exception("Плана не существует или он был удалён");
            }

            try
            {
                Db.WorkSchedules.Remove(schedule);
                Db.SaveChanges();
            }
            catch
            {
                throw new Exception("Невозможно удалить план, так как есть записи в циклограмме, ссылающиеся на мероприятия из этого плана. Обратитесь к администратору");
            }

        }

        public void AddAction(string userId, int workScheduleId, ActionViewModel action, ActionStatus status = ActionStatus.New)
        {
            var confirmationForm = Db.ConfirmationForms.FirstOrDefault(cf => cf.Id == action.ConfirmationFormId);

            if (confirmationForm == null)
            {
                throw new Exception("Необходимо указать форму подтверждения выполнения");
            }

            if (action.Date.ToShortDateString() == "01.01.0001")
            {
                throw new Exception("Необходимо указать дату проведения мероприятия");
            }

            if (String.IsNullOrWhiteSpace(action.Name))
            {
                throw new Exception("Необходимо ввести наименование мероприятия");
            }

            if (action.Responsibles.Count() == 0)
            {
                throw new Exception("Необходимо указать по крайней мере одного ответственного за данное мероприятие");
            }

            var newAction = new Models.Action
            {
                Date = action.Date,
                Name = action.Name,
                Status = status,
                ConfirmationFormId = confirmationForm.Id,
                WorkScheduleId = workScheduleId
            };

            Db.Actions.Add(newAction);
            Db.SaveChanges();

            var newTicket = new Ticket
            {
                UserId = userId,
                ActionId = newAction.Id,
                Comment = "Эта запись была создана автоматически после того, как вы добавили мероприятие в свой план",
                Date = newAction.Date,
                Hours = 8,
                Minutes = 0
            };

            Db.Tickets.Add(newTicket);
            Db.SaveChanges();

            var actionResponsiblesQuery = Db.ActionUsers;

            foreach (var user in action.Responsibles)
            {
                var actionUser = new ActionUser
                {
                    ActionId = newAction.Id,
                    UserId = user.Id
                };

                actionResponsiblesQuery.Add(actionUser);
            }

            Db.SaveChanges();
        }

        public void EditAction(ActionViewModel action)
        {
            var foundAction = Db.Actions.FirstOrDefault(a => a.Id == action.Id);

            if (foundAction == null)
            {
                throw new Exception("Мероприятия не существует. Возможно, оно было удалено");
            }

            if (action.Date.ToShortDateString() == "01.01.0001")
            {
                throw new Exception("Необходимо указать дату проведения мероприятия");
            }

            if (String.IsNullOrWhiteSpace(action.Name))
            {
                throw new Exception("Необходимо ввести наименование мероприятия");
            }

            if (action.Responsibles.Count() == 0)
            {
                throw new Exception("Необходимо указать по крайней мере одного ответственного за данное мероприятие");
            }

            foundAction.Date = action.Date;
            foundAction.Name = action.Name;
            foundAction.ConfirmationFormId = action.ConfirmationForm.Id;

            Db.SaveChanges();

            var actionResponsiblesQuery = Db.ActionUsers;

            var oldRecords = actionResponsiblesQuery.Where(ar => ar.ActionId == foundAction.Id);

            actionResponsiblesQuery.RemoveRange(oldRecords);

            Db.SaveChanges();

            foreach (var user in action.Responsibles)
            {
                var actionUser = new ActionUser
                {
                    ActionId = foundAction.Id,
                    UserId = user.Id
                };

                actionResponsiblesQuery.Add(actionUser);
            }

            Db.SaveChanges();
        }

        public void DeleteAction(int actionId)
        {
            var action = Db.Actions.FirstOrDefault(a => a.Id == actionId);
            var ticket = Db.Tickets.FirstOrDefault(t => t.ActionId == actionId);

            if (ticket == null)
            {
                var aus = Db.ActionUsers.Where(au => au.ActionId == actionId);
                Db.ActionUsers.RemoveRange(aus);
                Db.Actions.Remove(action);
            }
            else
            {
                action.IsDeleted = true;
            }

            Db.SaveChanges();
        }

        public void UpdateAction(ActionViewModel action)
        {
            var foundAction = Db.Actions.FirstOrDefault(a => a.Id == action.Id);

            foundAction.Date = action.Date;
            foundAction.Name = action.Name;
            foundAction.ConfirmationFormId = action.ConfirmationForm.Id;

            var actionResponsiblesQuery = Db.ActionUsers;
            var oldRecords = actionResponsiblesQuery.Where(au => au.ActionId == action.Id);
            actionResponsiblesQuery.RemoveRange(oldRecords);
            Db.SaveChanges();

            foreach (var user in action.Responsibles)
            {
                var actionUser = new ActionUser
                {
                    ActionId = action.Id,
                    UserId = user.Id
                };

                actionResponsiblesQuery.Add(actionUser);
            }

            Db.SaveChanges();
        }

        public Day MakeScheduleForDay(DateTime date, User responsible = null)
        {
            return MakeScheduleForPeriod(date, date, responsible)?.Days?.FirstOrDefault();
        }

        public GeneralScheduleViewModel MakeScheduleForPeriod(DateTime start, DateTime end, User responsible = null)
        {
            var actionUsers = Db.ActionUsers
                .Include(au => au.User)
                .Include(au => au.Action)
                .ToList();

            var groups = Db.Actions
                .Include(a => a.WorkSchedule)
                .Include(a => a.WorkSchedule.Activity)
                .Include(a => a.ConfirmationForm)
                .Where(a => a.Date.Date >= start.Date && a.Date.Date <= end.Date)
                .Where(a => !a.IsDeleted && a.Status == ActionStatus.Accepted)
                .OrderBy(a => a.Date)
                .ThenBy(a => a.WorkSchedule.Activity.Priority)
                .GroupBy(a => a.Date.Date)
                .AsEnumerable();

            if (groups == null)
            {
                return null;
            }

            var days = new List<Day>();

            foreach (var group in groups)
            {
                IEnumerable<Models.Action> query = group;

                if (responsible != null)
                {
                    query = group
                    .Where(action =>
                    {
                        var actionUser = actionUsers
                            .Where(au => au.ActionId == action.Id)
                            .FirstOrDefault(au => au.UserId == responsible.Id);
                        if (actionUser != null)
                            return true;
                        else
                            return false;
                    });
                }

                var actions = query
                    .Select(action => new ActionViewModel
                    {
                        Id = action.Id,
                        Name = action.Name,
                        Date = action.Date,
                        Status = action.Status,
                        Activity = new ActivityViewModel
                        {
                            Id = action.WorkSchedule.Activity.Id,
                            Name = action.WorkSchedule.Activity.Name,
                            Color = action.WorkSchedule.Activity.Color
                        },
                        ConfirmationForm = new ConfirmationFormViewModel
                        {
                            Id = action.ConfirmationForm.Id,
                            Name = action.ConfirmationForm.Name
                        },
                        Responsibles = actionUsers
                    .Where(ar => ar.ActionId == action.Id)
                    .Select(ar => new UserViewModel
                    {
                        Id = ar.User.Id,
                        Name = ar.User.UserName,
                        FirstName = ar.User.FirstName,
                        LastName = ar.User.LastName,
                        SurName = ar.User.SurName,
                    }),

                    });

                var day = new Day
                {
                    Date = group.Key,
                    ShortDayOfWeekName = culture.DateTimeFormat.GetShortestDayName(group.Key.DayOfWeek).ToLower(),
                    IsDayOff = (group.Key.DayOfWeek == DayOfWeek.Saturday || group.Key.DayOfWeek == DayOfWeek.Sunday) && actions.Count() == 0,
                    Actions = actions
                };

                days.Add(day);
            }

            for (DateTime i = start.Date; i <= end.Date; i = i.AddDays(1))
            {
                if (days.FirstOrDefault(d => d.Date.Date == i) == null)
                {
                    var day = new Day
                    {
                        Date = i,
                        ShortDayOfWeekName = culture.DateTimeFormat.GetShortestDayName(i.DayOfWeek).ToLower(),
                        Actions = null,
                        IsDayOff = i.DayOfWeek == DayOfWeek.Saturday || i.DayOfWeek == DayOfWeek.Sunday
                    };

                    if (!(day.IsDayOff == true && (day.Actions == null || day.Actions.Count() == 0)))
                    {
                        days.Add(day);
                    }
                }
            }

            days = days
                .OrderBy(d => d.Date)
                .ToList();

            var schedule = new GeneralScheduleViewModel
            {
                Days = days,
                Start = start,
                End = end
            };

            return schedule;
        }

        public async Task AllowConfirm(IEnumerable<int> actionIdsToAllowConfirm)
        {
            var actionsToAllowConfirm = Db.Actions.Where(a => actionIdsToAllowConfirm.Contains(a.Id));

            if (actionIdsToAllowConfirm.Count() > 0)
            {
                await actionsToAllowConfirm.ForEachAsync(a => a.Status = Models.Enums.ActionStatus.NeedConfirm);
                await Db.SaveChangesAsync();
                await NotificationService.NotifyToConfirmActions();
            }
        }

        public async Task Confirm(IEnumerable<int> actionIdsToConfirm)
        {
            var actionsToConfirm = Db.Actions.Where(a => actionIdsToConfirm.Contains(a.Id));

            if (actionsToConfirm.Count() > 0)
            {
                await actionsToConfirm.ForEachAsync(a => a.Status = Models.Enums.ActionStatus.Confirmed);
                await Db.SaveChangesAsync();
                await NotificationService.NotifyToAcceptActions();
            }
        }

        public async Task CancelConfirming(IEnumerable<int> actionIdsToCancel)
        {
            var actionsToCancel = Db.Actions.Where(a => actionIdsToCancel.Contains(a.Id));
            await actionsToCancel.ForEachAsync(a => a.Status = Models.Enums.ActionStatus.CanceledConfirming);
            await Db.SaveChangesAsync();
        }

        public async Task Accept(IEnumerable<int> actionIdsToAccept)
        {
            var actionsToAccept = Db.Actions.Where(a => actionIdsToAccept.Contains(a.Id));
            await actionsToAccept.ForEachAsync(a => a.Status = Models.Enums.ActionStatus.Accepted);
            await Db.SaveChangesAsync();
        }

        public async Task CancelAccepting(IEnumerable<int> actionIdsToCancel)
        {
            var actionsToCancel = Db.Actions.Where(a => actionIdsToCancel.Contains(a.Id));
            await actionsToCancel.ForEachAsync(a => a.Status = Models.Enums.ActionStatus.CanceledAccepting);
            await Db.SaveChangesAsync();
        }

        public IEnumerable<WorkScheduleViewModel> GetActionsToMake(ActionStatus targetStatus)
        {
            ActionStatus currentStatus = ActionStatus.New;

            switch (targetStatus)
            {
                case ActionStatus.Confirmed:
                    currentStatus = ActionStatus.NeedConfirm;
                    break;
                case ActionStatus.Accepted:
                    currentStatus = ActionStatus.Confirmed;
                    break;
            }

            var schedules = new List<WorkScheduleViewModel>();

            var actionUsers = Db.ActionUsers
               .Include(au => au.User)
               .Include(au => au.Action)
               .ToList();

            var scheduleGroups = Db.Actions
                .Include(a => a.WorkSchedule)
                .Include(a => a.ConfirmationForm)
                .Where(a => a.Status == currentStatus && !a.IsDeleted)
                .OrderBy(a => a.Date)
                .GroupBy(a => a.WorkSchedule);

            foreach (var group in scheduleGroups)
            {
                var schedule = new WorkScheduleViewModel();

                var actions = group.Select(a => new ActionViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Date = a.Date,
                    Status = a.Status,
                    ConfirmationForm = new ConfirmationFormViewModel
                    {
                        Id = a.ConfirmationForm.Id,
                        Name = a.ConfirmationForm.Name
                    },
                    Responsibles = actionUsers
                    .Where(ar => ar.ActionId == a.Id)
                    .Select(ar => new UserViewModel
                    {
                        Id = ar.User.Id,
                        Name = ar.User.UserName,
                        FirstName = ar.User.FirstName,
                        LastName = ar.User.LastName,
                        SurName = ar.User.SurName,
                    })
                    .ToList(),
                    ScheduleName = a.WorkSchedule.Name,
                    AuthorName = $"{a.WorkSchedule.User.LastName} {a.WorkSchedule.User.FirstName[0]}. {a.WorkSchedule.User.SurName[0]}."
                })
                .ToList();

                schedule.Id = group.Key.Id;
                schedule.Name = group.Key.Name;
                schedule.User = new UserViewModel
                {
                    Id = group.Key.User.Id,
                    Name = group.Key.User.UserName,
                    FullName = actions.First().AuthorName
                };
                schedule.Actions = actions;

                schedules.Add(schedule);
            }

            return schedules;
        }
    }
}
