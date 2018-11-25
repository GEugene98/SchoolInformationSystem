using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkScheduler.Models;
using WorkScheduler.Models.Identity;
using WorkScheduler.ViewModels;

namespace WorkScheduler.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class TicketController : Controller
    {
        protected Context Db;
        protected UserManager<User> UserManager;

        public TicketController(Context context, UserManager<User> userManager)
        {
            Db = context;
            UserManager = userManager;
        }

        [HttpPost("MyTickets")]
        public IActionResult MyTickets([FromBody]DateTime dateTo)
        {
            dateTo = dateTo.AddHours(3);
            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);
            var actionUsers = Db.ActionUsers
                .Include(au => au.User)
                .Include(au => au.Action)
                .ToList();
            var tickets = Db.Tickets
                .Include(t => t.Action)
                .Include(t => t.Action.ConfirmationForm)
                .Include(t => t.Action.WorkSchedule)
                .Include(t => t.Action.WorkSchedule.Activity)
                .Where(t => t.UserId == currentUser.Id);

            if (dateTo == null || dateTo.ToShortDateString() == "01.01.0001")
            {
                dateTo = DateTime.Now;
            }

            var groupedTickets = tickets
                .Where(t => t.Date.Date == dateTo.Date)
                .ToList()
                .OrderBy(t => t.Date.Date)
                .GroupBy(t => t.Date.Date);

            var ticketPacks = new List<TicketPackViewModel>();

            foreach (var group in groupedTickets)
            {
                var ticketsToPack = group.Select(t => new TicketViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Date = t.Date,
                    Comment = t.Comment,
                    Done = t.Done,
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
                    Minutes = t.Minutes ?? 0
                });

                var timeGroups = new List<TicketTimeGroup>();
                var minTicketHour = ticketsToPack.Select(ttp => ttp.Hours).Min().Value;
                var startHour = minTicketHour < 8 ? minTicketHour : 8;
                var maxTicketHour = ticketsToPack.Select(ttp => ttp.Hours).Max().Value;
                var endHour = maxTicketHour > 17 ? maxTicketHour : 17;

                for (var i = startHour; i <= endHour; i++)
                {
                    var timeGroup = new TicketTimeGroup
                    {
                        Hour = i,
                        Tickets = ticketsToPack.Where(t => t.Hours == i).ToList()
                    };

                    timeGroups.Add(timeGroup);
                }

                var pack = new TicketPackViewModel
                {
                    Date = group.Key,
                    TimeGroups = timeGroups
                };

                ticketPacks.Add(pack);
            }

            return Ok(ticketPacks);
        }

        [HttpPost("Add")]
        public IActionResult Add([FromBody]TicketViewModel ticket)
        {
            if (ticket == null || ticket.Date.ToShortDateString() == "01.01.0001" || (ticket.Hours == null && ticket.Minutes == null))
            {
                return BadRequest("Необходимо указать дату и время");
            }

            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);

            var newTicket = new Ticket
            {
                Name = ticket.Name,
                ActionId = ticket.Action?.Id,
                UserId = currentUser.Id,
                Comment = ticket.Comment,
                Done = ticket.Done,
                Date = ticket.Date.AddHours(3),
                Hours = ticket.Hours == null ? 0 : ticket.Hours,
                Minutes = ticket.Minutes == null ? 0 : ticket.Minutes
            };

            Db.Tickets.Add(newTicket);

            Db.SaveChanges();

            return Ok();
        }

        [HttpPost("Update")]
        public IActionResult Update([FromBody]TicketViewModel ticket)
        {
            var foundTicket = Db.Tickets.FirstOrDefault(t => t.Id == ticket.Id);
            foundTicket.Name = ticket.Name;
            foundTicket.Comment = ticket.Comment;
            foundTicket.Date = ticket.Date.AddHours(3);
            foundTicket.Done = ticket.Done;
            foundTicket.Hours = ticket.Hours;
            foundTicket.Minutes = ticket.Minutes;
            Db.SaveChanges();
            return Ok();
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(long ticketId)
        {
            var ticket = Db.Tickets.FirstOrDefault(t => t.Id == ticketId);
            Db.Tickets.Remove(ticket);
            Db.SaveChanges();
            return Ok();
        }
    }
}
