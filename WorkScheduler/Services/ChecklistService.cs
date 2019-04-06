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
    public class ChecklistService
    {
        protected Context Db;
        protected CultureInfo culture = CultureInfo.GetCultureInfo("ru-RU");

        public ChecklistService(Context context)
        {
            Db = context;
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
                        DoneCount = done.Where(t => t.Key == c.Id).Count()
                    };

                checklistViewModels.Add(clModel);
            }

            return checklistViewModels;
        }
    }
}