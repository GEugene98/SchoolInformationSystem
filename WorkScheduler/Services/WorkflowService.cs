using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WorkScheduler.Models;
using WorkScheduler.Models.Workflow;
using WorkScheduler.ViewModels;

namespace WorkScheduler.Services
{
    public class WorkflowService
    {
        protected Context Db;

        public WorkflowService(Context context)
        {
            Db = context;
        }

        public List<IncomingDocumentViewModel> GetIncomingDocuments(int schoolId)
        {
            var docs = Db.IncomingDocuments
                .Include(d => d.Organization)
                .Include(d => d.User)
                .Where(d => d.SchoolId == schoolId);

            var foundFiles =
                (
                    from d in docs
                    join df in Db.IncomingDocumentFiles on d.Id equals df.IncomingDocumentId
                    join f in Db.Files on df.FileId equals f.Id
                    select new
                    {
                        IncomingDocumentId = d.Id,
                        File = f
                    }
                )
                .ToList();

            var result = docs
                .Select(d => new IncomingDocumentViewModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    Num = d.Num,
                    User = new UserViewModel
                    {
                        Id = d.User.Id,
                        FullName = d.User.LastName + ' ' + d.User.FirstName[0] + ". " + d.User.SurName[0]
                    },
                    Type = d.Type,
                    Taken = d.Taken,
                    Organization = new DictionaryViewModel<int>
                    {
                        Id = d.Organization.Id,
                        Name = d.Organization.Name
                    },
                    OrganizationId = d.Organization.Id,
                    Deadline = d.Deadline,
                    Done = d.Done,
                    Description = d.Description,
                    Files = foundFiles
                    .Where(f => f.IncomingDocumentId == d.Id)
                    .Select(f => new FileViewModel
                    {
                        Id = f.File.Id,
                        Name = f.File.Name,
                        SizeMb = f.File.SizeMb,
                        Extension = f.File.Extension
                    })
                })
                .OrderByDescending(d => d.Taken)
                .ThenByDescending(d => d.Num)
                .ToList();

            return result;
        }

        public List<OutgoingDocumentViewModel> GetOutgoingDocuments(int schoolId)
        {
            var docs = Db.OutgoingDocuments
                .Include(d => d.Organization)
                .Include(d => d.User)
                .Where(d => d.SchoolId == schoolId);

            var foundFiles =
                (
                    from d in docs
                    join df in Db.OutgoingDocumentFiles on d.Id equals df.OutgoingDocumentId
                    join f in Db.Files on df.FileId equals f.Id
                    select new
                    {
                        OutgoingDocumentId = d.Id,
                        File = f
                    }
                )
                .ToList();

            var result = docs
                .Select(d => new OutgoingDocumentViewModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    Num = d.Num,
                    User = new UserViewModel
                    {
                        Id = d.User.Id,
                        FullName = d.User.LastName + ' ' + d.User.FirstName[0] + ". " + d.User.SurName[0]
                    },
                    Type = d.Type,
                    Taken = d.Taken,
                    Organization = new DictionaryViewModel<int>
                    {
                        Id = d.Organization.Id,
                        Name = d.Organization.Name
                    },
                    OrganizationId = d.Organization.Id,
                    Deadline = d.Deadline,
                    Done = d.Done,
                    Description = d.Description,
                    Files = foundFiles
                    .Where(f => f.OutgoingDocumentId == d.Id)
                    .Select(f => new FileViewModel
                    {
                        Id = f.File.Id,
                        Name = f.File.Name,
                        SizeMb = f.File.SizeMb,
                        Extension = f.File.Extension
                    })
                })
                .OrderByDescending(d => d.Taken)
                .ThenByDescending(d => d.Num)
                .ToList();

            return result;
        }

        public int CreateIncomingDocument(IncomingDocumentViewModel document, int schoolId)
        {
            var newDoc = new IncomingDocument
            {
                Name = document.Name,
                Num = document.Num,
                Deadline = document.Deadline,
                Description = document.Description,
                Done = document.Done,
                OrganizationId = document.OrganizationId,
                SchoolId = schoolId,
                Taken = document.Taken,
                Type = document.Type,
                UserId = document.UserId
            };

            Db.IncomingDocuments.Add(newDoc);
            Db.SaveChanges();

            if (document.CreateTicket)
            {
                var checklistId = Db.Schools.FirstOrDefault(s => s.Id == schoolId).IncomingWorkflowChecklist;

                var responsibleTicket = new Ticket
                {
                    Name = document.Name,
                    Date = document.Deadline,
                    ChecklistId = Convert.ToInt32(checklistId),
                    Comment = document.Description,
                    UserId = document.UserId,
                    Status = Models.Enums.TicketStatus.Assigned,
                    IncomingDocumentId = newDoc.Id,
                };

                Db.Tickets.Add(responsibleTicket);

                foreach (var item in document.UserIdsToCheck)
                {
                    var ticket = new Ticket
                    {
                        Name = document.Name,
                        Date = document.Deadline,
                        ChecklistId = Convert.ToInt32(checklistId),
                        Comment = document.Description,
                        UserId = item,
                        Status = Models.Enums.TicketStatus.Assigned,
                        IncomingDocumentId = newDoc.Id,
                        OnCheck = true
                    };

                    Db.Tickets.Add(ticket);
                }

                Db.SaveChanges();
            }

            return newDoc.Id;
        }

        public int CreateOutgoingDocument(OutgoingDocumentViewModel document, int schoolId)
        {
            var newDoc = new OutgoingDocument
            {
                Name = document.Name,
                Num = document.Num,
                Deadline = document.Deadline,
                Description = document.Description,
                Done = document.Done,
                OrganizationId = document.OrganizationId,
                SchoolId = schoolId,
                Taken = document.Taken,
                Type = document.Type,
                UserId = document.UserId
            };

            Db.OutgoingDocuments.Add(newDoc);
            Db.SaveChanges();

            if (document.CreateTicket)
            {

                var checklistId = Db.Schools.FirstOrDefault(s => s.Id == schoolId).OutgoingWorkflowChecklist;

                var responsibleTicket = new Ticket
                {
                    Name = document.Name,
                    Date = document.Deadline,
                    ChecklistId = Convert.ToInt32(checklistId),
                    Comment = document.Description,
                    UserId = document.UserId,
                    Status = Models.Enums.TicketStatus.Assigned,
                    OutgoingDocumentId = newDoc.Id,
                };

                Db.Tickets.Add(responsibleTicket);

                foreach (var item in document.UserIdsToCheck)
                {
                    var ticket = new Ticket
                    {
                        Name = document.Name,
                        Date = document.Deadline,
                        ChecklistId = Convert.ToInt32(checklistId),
                        Comment = document.Description,
                        UserId = item,
                        Status = Models.Enums.TicketStatus.Assigned,
                        OutgoingDocumentId = newDoc.Id,
                        OnCheck = true
                    };

                    Db.Tickets.Add(ticket);
                }

                Db.SaveChanges();

            }

            return newDoc.Id;
        }

        public void UpdateIncomingDocument(IncomingDocumentViewModel document)
        {
            var found = Db.IncomingDocuments.FirstOrDefault(d => d.Id == document.Id);

            if (found == null)
            {
                throw new Exception("Документ не найден");
            }

            found.Name = document.Name;
            found.Num = document.Num;
            found.OrganizationId = document.OrganizationId;
            found.Taken = document.Taken;
            found.Type = document.Type;
            found.Done = document.Done;
            found.Description = document.Description;
            found.Deadline = document.Deadline;

            Db.SaveChanges();
        }

        public void UpdateOutgoingDocument(OutgoingDocumentViewModel document)
        {
            var found = Db.OutgoingDocuments.FirstOrDefault(d => d.Id == document.Id);

            if (found == null)
            {
                throw new Exception("Документ не найден");
            }

            found.Name = document.Name;
            found.Num = document.Num;
            found.OrganizationId = document.OrganizationId;
            found.Taken = document.Taken;
            found.Type = document.Type;
            found.Done = document.Done;
            found.Description = document.Description;
            found.Deadline = document.Deadline;

            Db.SaveChanges();
        }

        public void DeleteIncomingDocument(int id)
        {
            var found = Db.IncomingDocuments.FirstOrDefault(d => d.Id == id);
            if (found == null)
            {
                throw new Exception("Документ не найден");
            }
            Db.IncomingDocuments.Remove(found);
            Db.SaveChanges();
        }

        public void DeleteOutgoingDocument(int id)
        {
            var found = Db.OutgoingDocuments.FirstOrDefault(d => d.Id == id);
            if (found == null)
            {
                throw new Exception("Документ не найден");
            }
            Db.OutgoingDocuments.Remove(found);
            Db.SaveChanges();
        }

    }
}
