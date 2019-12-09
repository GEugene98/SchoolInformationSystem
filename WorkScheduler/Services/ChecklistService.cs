using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WorkScheduler.Models.Enums;
using WorkScheduler.Models.Identity;
using WorkScheduler.Models.Scheduler;
using WorkScheduler.ViewModels;
using WorkScheduler.ViewModels.Scheduler;

namespace WorkScheduler.Services
{
    public class ChecklistService
    {
        protected Context Db;
        protected CultureInfo culture = CultureInfo.GetCultureInfo("ru-RU");

        public ChecklistService(Context context)
        {
            Db = context;
        }

        public ChecklistViewModel GetChecklistById(int id)
        {
            var checklist = Db.Checklists.FirstOrDefault(c => c.Id == id);

            if (checklist == null)
            {
                throw new Exception("Такого чек-листа не существует");
            }

            var ticketsQuery = Db.Tickets
                .Where(t => t.ChecklistId == id)
                .Include(t => t.User)
                .OrderBy(t => t.Date)
                .ThenBy(t => t.Hours)
                .ThenBy(t => t.Minutes);

            var foundFiles = 
                (
                    from t in ticketsQuery
                    join tf in Db.TicketFiles on t.Id equals tf.TicketId
                    join f in Db.Files on tf.FileId equals f.Id
                    select new 
                    {
                        TicketId = t.Id,
                        Type = tf.Type,
                        File = f
                    }
                )
                .ToList();

            var tickets = ticketsQuery
                .Select(t => new TicketViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    User = new UserViewModel
                    {
                        Id = t.User.Id,
                        Name = t.User.UserName,
                        FirstName = t.User.FirstName,
                        LastName = t.User.LastName,
                        SurName = t.User.SurName,
                        FullName = $"{t.User.LastName} {t.User.FirstName[0]}.{t.User.SurName[0]}."
                    },
                    AssignmentComment = t.AssignmentComment,
                    Comment = t.Comment,
                    Date = t.Date,
                    Hours = t.Hours,
                    Minutes = t.Minutes,
                    Created = t.Created,
                    Status = t.Status,
                    Done = t.Done,
                    ResponseComment = t.ResponseComment,
                    IsExpiered = t.Date.HasValue && DateTime.Now.Date > t.Date.Value.Date && t.Status != TicketStatus.Done,
                    InFiles = foundFiles
                        .Where(f => f.TicketId == t.Id && f.Type == TicketFileType.Incoming)
                        .Select (f => new FileViewModel
                                        {
                                            Id = f.File.Id,
                                            Name = f.File.Name,
                                            SizeMb = f.File.SizeMb,
                                            Extension = f.File.Extension
                                        }
                                ),
                    OutFiles = foundFiles
                        .Where(f => f.TicketId == t.Id && f.Type == TicketFileType.Outgoing)
                        .Select (f => new FileViewModel
                                        {
                                            Id = f.File.Id,
                                            Name = f.File.Name,
                                            SizeMb = f.File.SizeMb,
                                            Extension = f.File.Extension
                                        }
                                )
                })
                .ToList();

            return new ChecklistViewModel
            {
                Id = checklist.Id,
                Name = checklist.Name,
                CreatedOn = checklist.CreatedOn,
                Deadline = checklist.Deadline,
                Comment = checklist.Comment,
                Tickets = tickets
            };
        }

        public IEnumerable<ChecklistViewModel> GetOtherChecklists(int schoolId, params string[] userIdsToExclude)
        {
            var usersHavingChecklists = Db.Users
                .Where(u => u.Checklists.Any() && !userIdsToExclude.Contains(u.Id) && u.SchoolId == schoolId);

            var otherChecklists = new List<ChecklistViewModel>();

            foreach (var user in usersHavingChecklists)
            {
                var checklists = GetChecklists(user.Id);
                otherChecklists.AddRange(checklists);
            }

            return otherChecklists.OrderBy(i => i.User.LastName);
        }

        public IEnumerable<ChecklistViewModel> GetChecklists(string userId)
        {
            var checklists = Db.Checklists
                .Where(c => c.UserId == userId);

            var total = checklists
                .Join(Db.Tickets, c => c.Id, t => t.ChecklistId, (c, t) => new KeyValuePair<int, long>(c.Id, t.Id)).ToList();
            var assigned = checklists
                .Join(Db.Tickets.Where(t => t.Status == TicketStatus.Assigned), c => c.Id, t => t.ChecklistId, (c, t) => new KeyValuePair<int, long>(c.Id, t.Id)).ToList();
            var accepted = checklists
                .Join(Db.Tickets.Where(t => t.Status == TicketStatus.Accepted), c => c.Id, t => t.ChecklistId, (c, t) => new KeyValuePair<int, long>(c.Id, t.Id)).ToList();
            var done = checklists
                .Join(Db.Tickets.Where(t => t.Status == TicketStatus.Done), c => c.Id, t => t.ChecklistId, (c, t) => new KeyValuePair<int, long>(c.Id, t.Id)).ToList();
            var expiered = checklists
                .Join(Db.Tickets.Where(t => t.Status != TicketStatus.Done && t.Date.HasValue && DateTime.Now.Date > t.Date.Value.Date && t.Status != TicketStatus.Declined), c => c.Id, t => t.ChecklistId, (c, t) => new KeyValuePair<int, long>(c.Id, t.Id)).ToList();
            var acceptedExpiered = checklists
                .Join(Db.Tickets.Where(t => t.Status == TicketStatus.Accepted && t.Date.HasValue && DateTime.Now.Date > t.Date.Value.Date), c => c.Id, t => t.ChecklistId, (c, t) => new KeyValuePair<int, long>(c.Id, t.Id)).ToList();

            var checklistViewModels = new List<ChecklistViewModel>();

            var usersChecklists = checklists.ToList();

            foreach (var c in usersChecklists)
            {
                var clModel =
                    new ChecklistViewModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        CreatedOn = c.CreatedOn,
                        Deadline = c.Deadline,
                        Comment = c.Comment,
                        TotalCount = total.Where(t => t.Key == c.Id).Count(),
                        AssignedCount = assigned.Where(t => t.Key == c.Id).Count(),
                        AcceptedCount = accepted.Where(t => t.Key == c.Id).Count(),
                        DoneCount = done.Where(t => t.Key == c.Id).Count(),
                        ExpieredCount = expiered.Where(t => t.Key == c.Id).Count(),
                        AcceptedExpieredCount = acceptedExpiered.Where(t => t.Key == c.Id).Count(),
                        User = new UserViewModel
                        {
                            Id = c.User.Id,
                            Name = c.User.UserName,
                            FirstName = c.User.FirstName,
                            LastName = c.User.LastName,
                            SurName = c.User.SurName,
                            FullName = $"{c.User.LastName} {c.User.FirstName} {c.User.SurName}",
                        }
                    };

                checklistViewModels.Add(clModel);
            }

            return checklistViewModels;
        }

        public void AddChecklist(ChecklistViewModel checklist, string userId)
        {
            if (!checklist.Deadline.HasValue) throw new Exception("Укажите срок выполнения");

            var newChecklist = new Checklist
            {
                Name = checklist.Name,
                UserId = userId,
                Deadline = checklist.Deadline,
                CreatedOn = DateTime.Now,
                Comment = checklist.Comment
            };

            Db.Checklists.Add(newChecklist);
            Db.SaveChanges();
        }

        public void DeleteChecklist(int checklistId)
        {
            var foundChecklist = Db.Checklists.FirstOrDefault(c => c.Id == checklistId);

            if (foundChecklist == null)
            {
                throw new Exception("Чек-лист не найден");
            }

            var tickets = Db.Tickets.Where(t => t.ChecklistId == foundChecklist.Id);

            Db.Tickets.RemoveRange(tickets);
            Db.Checklists.Remove(foundChecklist);

            Db.SaveChanges();
        }

        public void EditChecklist(ChecklistViewModel checklist)
        {
            var foundChecklist = Db.Checklists.FirstOrDefault(c => c.Id == checklist.Id);

            if (foundChecklist == null)
            {
                throw new Exception("Чек-лист не найден");
            }

            foundChecklist.Name = checklist.Name;
            foundChecklist.Comment = checklist.Comment;
            foundChecklist.Deadline = checklist.Deadline;
            Db.SaveChanges();
        }
    }
}