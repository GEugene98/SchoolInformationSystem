using System;
using System.Collections.Generic;

namespace WorkScheduler.ViewModels
{
    public class OutgoingDocumentViewModel : DictionaryViewModel<int>
    {
        public int SchoolId { get; set; }
        public DictionaryViewModel<int> School { get; set; }

        public string UserId { get; set; }
        public UserViewModel User { get; set; }

        public List<TicketViewModel> Tickets { get; set; }

        public List<string> UserIdsToCheck { get; set; }

        public string Num { get; set; }
        public int OrganizationId { get; set; }
        public DictionaryViewModel<int> Organization { get; set; }
        public string Type { get; set; }
        public DateTime? Taken { get; set; }
        public DateTime? Deadline { get; set; }
        public bool Done { get; set; }
        public string Description { get; set; }

        public IEnumerable<FileViewModel> Files { get; set; }
        public IEnumerable<FileViewModel> FilesFromTickets { get; set; }

        public bool CreateTicket { get; set; }
    }
}
