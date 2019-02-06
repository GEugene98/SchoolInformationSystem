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
using WorkScheduler.Services;
using WorkScheduler.ViewModels;

namespace WorkScheduler.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class TicketController : Controller
    {
        protected Context Db;
        protected UserManager<User> UserManager;
        protected NotificationService NotificationService;
        protected TicketService TicketService;
        private readonly Logger Logger;

        public TicketController(Context context, UserManager<User> userManager, NotificationService notificationService, TicketService ticketService)
        {
            Db = context;
            UserManager = userManager;
            NotificationService = notificationService;
            TicketService = ticketService;
            Logger = Logger.GetInstance();
        }

        [HttpPost("MyTickets")]
        public IActionResult MyTickets([FromBody]IEnumerable<DateTime> range)
        {
            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);

            var ticketPacks = TicketService.GetTickets(range, currentUser);

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

            if (ticket.Repeat && ticket.Days != null)
            {
                var daysOfWeek = ticket.Days.Cast<DayOfWeek>();

                while (ticket.Date.Date <= ticket.DateTo.Date)
                {
                    if (daysOfWeek.Contains(ticket.Date.DayOfWeek))
                    {
                        AddTicket(ticket);
                    }

                    ticket.Date = ticket.Date.AddDays(1);
                }

            }
            else
            {
                AddTicket(ticket);
            }

            void AddTicket(TicketViewModel ticketModel)
            {
                var newTicket = new Ticket
                {
                    Name = ticketModel.Name,
                    ActionId = ticketModel.Action?.Id,
                    UserId = currentUser.Id,
                    Comment = ticketModel.Comment,
                    Done = ticketModel.Done,
                    Date = ticketModel.Date.AddHours(3),
                    Hours = ticketModel.Hours == null ? 0 : ticket.Hours,
                    Minutes = ticketModel.Minutes == null ? 0 : ticket.Minutes
                };

                Db.Tickets.Add(newTicket);
            }

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

        [HttpPost("SendTimeline")]
        public IActionResult SendTimeline([FromBody]IEnumerable<DateTime> range)
        {
            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);

            try
            {
                var ticketPacks = TicketService.GetTickets(range, currentUser);
                NotificationService.SendTimeline(ticketPacks, currentUser);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpGet("MakeDone")]
        public IActionResult MakeDone(int ticketId)
        {
            try
            {
                var ticket = Db.Tickets.FirstOrDefault(t => t.Id == ticketId);

                if (ticket == null)
                {
                    throw new Exception("Запись не найдена");
                }

                ticket.Done = !ticket.Done;
                Db.SaveChanges();

                var message = ticket.Done ? "Запись помечена как выполненная" : "Пометка \"Выполнено\" снята";

                return Ok(new { message });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("MakeImportant")]
        public IActionResult MakeImportant(int ticketId)
        {
            try
            {
                var ticket = Db.Tickets.FirstOrDefault(t => t.Id == ticketId);

                if (ticket == null)
                {
                    throw new Exception("Запись не найдена");
                }

                ticket.Important = !ticket.Important;
                Db.SaveChanges();

                var message = ticket.Important ? "Запись помечена как важная" : "Пометка \"Важно\" снята";

                return Ok(new { message });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
