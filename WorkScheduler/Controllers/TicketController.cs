using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkScheduler.Models;
using WorkScheduler.Models.Enums;
using WorkScheduler.Models.Identity;
using WorkScheduler.Models.Shared;
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
        private FileService FileService { get; set; }
        private readonly Logger Logger;

        public TicketController(Context context, UserManager<User> userManager, NotificationService notificationService, TicketService ticketService, FileService fileService)
        {
            Db = context;
            UserManager = userManager;
            NotificationService = notificationService;
            TicketService = ticketService;
            Logger = Logger.GetInstance();
            FileService = fileService;
        }

        [HttpPost("MyTickets")]
        public IActionResult MyTickets([FromBody]IEnumerable<DateTime> range)
        {
            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);

            var ticketPacks = TicketService.GetTickets(range, currentUser);

            return Ok(ticketPacks);
        }

        [HttpGet("AssignedTickets")]
        public IActionResult AssignedTickets()
        {
            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);

            var tickets = TicketService.GetAssignedTickets(currentUser.Id);

            return Ok(tickets);
        }

        [HttpPost("Add")]
        public IActionResult Add([FromBody]TicketViewModel ticket)
        {
            if (ticket == null || !ticket.Date.HasValue || ticket.Date.Value.ToShortDateString() == "01.01.0001" || (ticket.Hours == null && ticket.Minutes == null))
            {
                return BadRequest("Необходимо указать дату и время");
            }

            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);

            if (ticket.Repeat && ticket.Days != null)
            {
                var daysOfWeek = ticket.Days.Cast<DayOfWeek>();

                while (ticket.Date.Value.Date <= ticket.DateTo.Date)
                {
                    if (daysOfWeek.Contains(ticket.Date.Value.DayOfWeek))
                    {
                        AddTicket(ticket);
                    }

                    ticket.Date = ticket.Date.Value.AddDays(1);
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
                    Date = ticketModel.Date.Value.AddHours(3),
                    Hours = ticketModel.Hours == null ? 0 : ticket.Hours,
                    Minutes = ticketModel.Minutes == null ? 0 : ticket.Minutes
                };

                Db.Tickets.Add(newTicket);
            }

            Db.SaveChanges();
            return Ok();
        }

        [HttpPost("AddFromChecklist")]
        public IActionResult AddFromChecklist([FromBody] TicketViewModel ticket)
        {
            string transactionId = Request.Headers["transaction-id"];
            var schoolId = Db.Users.First(u => u.UserName == this.User.Identity.Name).SchoolId.ToString();

            var uploadedFiles = FileService.PutFilesInDb(transactionId, schoolId);

            if (ticket.UserIdsToAssignTicket == null || ticket.UserIdsToAssignTicket.Count() == 0)
            {
                var newTicket = new Ticket
                {
                    Name = ticket.Name,
                    Comment = ticket.Comment,
                    Hours = ticket.Hours,
                    Minutes = ticket.Minutes,
                    ChecklistId = ticket.ChecklistId,
                    Status = TicketStatus.Created
                };

                if (ticket.Date.HasValue)
                {
                    newTicket.Date = ticket.Date.Value.AddHours(3);
                }

                Db.Tickets.Add(newTicket);
                Db.SaveChanges();

                if (uploadedFiles.Count() != 0)
                {
                    FileService.BindFilesToTicket(uploadedFiles, newTicket.Id, TicketFileType.Incoming);
                }

                return Ok();
            }

            foreach (var userId in ticket.UserIdsToAssignTicket)
            {
                var newTicket = new Ticket
                {
                    Name = ticket.Name,
                    Comment = ticket.Comment,
                    Hours = ticket.Hours,
                    Minutes = ticket.Minutes,
                    ChecklistId = ticket.ChecklistId,
                    Status = TicketStatus.Assigned,
                    UserId = userId
                };

                if (ticket.Date.HasValue)
                {
                    newTicket.Date = ticket.Date.Value.AddHours(3);
                }

                Db.Tickets.Add(newTicket);
                Db.SaveChanges();

                if (uploadedFiles.Count() != 0)
                {
                    FileService.BindFilesToTicket(uploadedFiles, newTicket.Id, TicketFileType.Incoming);
                }
            }

            return Ok();
        }

        [HttpGet("DeleteFileBinding")]
        public IActionResult DeleteFileBinding(long fileId, long ticketId, TicketFileType type)
        {
            var bindings = Db.TicketFiles.Where(tf => tf.FileId == fileId && tf.TicketId == ticketId && tf.Type == type);
            Db.TicketFiles.RemoveRange(bindings);
            Db.SaveChanges();
            return Ok();
        }

        [HttpPost("SaveReply")]
        public IActionResult SaveReply([FromBody] TicketViewModel ticket, string transactionId)
        {
            var foundTicket = Db.Tickets.FirstOrDefault(t => t.Id == ticket.Id);

            if (foundTicket == null)
            {
                return BadRequest("Запись не найдена");
            }

            foundTicket.ResponseComment = ticket.ResponseComment;

            var schoolId = Db.Users.First(u => u.UserName == this.User.Identity.Name).SchoolId.ToString();

            Db.SaveChanges();

            var uploadedFiles = FileService.PutFilesInDb(transactionId, schoolId);

            if (uploadedFiles.Count() != 0)
            {
                FileService.BindFilesToTicket(uploadedFiles, foundTicket.Id, TicketFileType.Outgoing);
            }

            return Ok();
        }

        [HttpGet("SaveChecklistTicketDetails")]
        public IActionResult SaveChecklistTicketDetails(long ticketId, string transactionId)
        {
            var foundTicket = Db.Tickets.FirstOrDefault(t => t.Id == ticketId);

            if (foundTicket == null)
            {
                return BadRequest("Запись не найдена");
            }

            var schoolId = Db.Users.First(u => u.UserName == this.User.Identity.Name).SchoolId.ToString();

            var uploadedFiles = FileService.PutFilesInDb(transactionId, schoolId);

            if (uploadedFiles.Count() != 0)
            {
                FileService.BindFilesToTicket(uploadedFiles, foundTicket.Id, TicketFileType.Incoming);
            }

            return Ok();
        }

        [HttpPost("EditFromChecklist")]
        public IActionResult EditFromChecklist([FromBody] TicketViewModel ticket)
        {
            var found = Db.Tickets.FirstOrDefault(t => t.Id == ticket.Id);

            if (found == null) return BadRequest("Задания не существует");

            found.Date = ticket.Date;
            found.Hours = ticket.Hours;
            found.Minutes = ticket.Minutes;
            found.Comment = ticket.Comment;
            found.Name = ticket.Name;

            if (found.UserId != ticket.UserId)
            {
                found.UserId = ticket.UserId;
                found.Status = TicketStatus.Assigned;
                found.Done = false;
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
            foundTicket.Date = ticket.Date.Value.AddHours(3);
            foundTicket.Done = ticket.Done;
            foundTicket.Hours = ticket.Hours;
            foundTicket.Minutes = ticket.Minutes;
            Db.SaveChanges();
            return Ok();
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(long ticketId, bool deleteAll = false)
        {
            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);

            var ticket = Db.Tickets.FirstOrDefault(t => t.Id == ticketId);

            if (ticket.ChecklistId != null)
            {
                var checklistName = Db.Checklists.FirstOrDefault(c => c.Id == ticket.ChecklistId).Name;
                return BadRequest($@"Вы не можете удалить эту запись, так как она находится в чек-листе ""{checklistName}"". Если этот чек-лист ваш, удалите запись из него");
            }

            if (deleteAll)
            {
                var similar = Db.Tickets.Where(t => t.Name == ticket.Name && t.UserId == currentUser.Id).ToList();
                Db.Tickets.RemoveRange(similar);
            }
            else
            {
                Db.Tickets.Remove(ticket);
            }

            Db.SaveChanges();

            return Ok();
        }

        [HttpDelete("DeleteFromChecklist")]
        public IActionResult DeleteFromChecklist(long id)
        {
            var ticket = Db.Tickets.FirstOrDefault(t => t.Id == id);
            if (ticket == null) return BadRequest("Задания не существует");
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
        public IActionResult MakeDone(long ticketId)
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

                var message = ticket.Done ? "Запись помечена как выполненная" : "Отметка \"Выполнено\" снята";

                return Ok(new { message });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("MakeDoneChecklistTicket")]
        public IActionResult MakeDoneChecklistTicket(long ticketId)
        {
            try
            {
                var ticket = Db.Tickets.FirstOrDefault(t => t.Id == ticketId);

                if (ticket == null)
                {
                    throw new Exception("Запись не найдена. Обновите страницу");
                }

                if (ticket.Status == Models.Enums.TicketStatus.Accepted)
                {
                    ticket.Status = Models.Enums.TicketStatus.Done;
                    ticket.Done = true;
                }
                else if (ticket.Status == Models.Enums.TicketStatus.Done)
                {
                    ticket.Status = Models.Enums.TicketStatus.Accepted;
                    ticket.Done = false;
                }

                Db.SaveChanges();

                var message = ticket.Done ? "Запись помечена как выполненная" : "Отметка \"Выполнено\" снята";

                return Ok(new { message });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("MakeDoneFromChecklistDetails")]
        public IActionResult MakeDoneFromChecklistDetails(long ticketId)
        {
            try
            {
                var ticket = Db.Tickets.FirstOrDefault(t => t.Id == ticketId);

                if (ticket == null)
                {
                    throw new Exception("Запись не найдена. Обновите страницу");
                }

                if (!ticket.Done)
                {
                    ticket.Status = Models.Enums.TicketStatus.Done;
                    ticket.Done = true;
                }
                else
                {
                    ticket.Status = Models.Enums.TicketStatus.Assigned;
                    ticket.Done = false;
                }

                Db.SaveChanges();

                var message = ticket.Done ? "Запись помечена как выполненная" : "Отметка \"Выполнено\" снята";

                return Ok(new { message });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("MakeImportant")]
        public IActionResult MakeImportant(long ticketId)
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

                var message = ticket.Important ? "Запись помечена как важная" : "Отметка \"Важно\" снята";

                return Ok(new { message });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("SimilarTickets")]
        public IActionResult SimilarTickets(long ticketId)
        {
            try
            {
                var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);
                var ticket = Db.Tickets.FirstOrDefault(t => t.Id == ticketId);

                var similarTickets = Db.Tickets.Where(t => t.Name == ticket.Name && t.Id != ticket.Id && t.UserId == currentUser.Id);

                if (similarTickets.Count() == 0)
                {
                    return Ok(new List<object>());
                }

                var tickets =
                    similarTickets
                    .OrderBy(t => t.Date)
                    .ToList()
                    .Select(t => new
                    {
                        t.Date,
                        t.Name,
                        t.Comment,
                        t.Done,
                        t.Important
                    });

                return Ok(tickets);

            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost("AcceptTicket")]
        public IActionResult AcceptTicket([FromBody]TicketViewModel ticket)
        {
            try
            {
                TicketService.AcceptTicket(ticket.Id, ticket.Date.Value.AddHours(3), ticket.Hours, ticket.Minutes);
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("DeclineTicket")]
        public IActionResult DeclineTicket(long id)
        {
            try
            {
                var ticket = Db.Tickets.FirstOrDefault(t => t.Id == id);
                if (ticket == null)
                {
                    throw new Exception("Задание было удалено");
                }
                ticket.Status = TicketStatus.Declined;
                Db.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                return BadRequest(ex.Message);
            }
        }
    }
}
