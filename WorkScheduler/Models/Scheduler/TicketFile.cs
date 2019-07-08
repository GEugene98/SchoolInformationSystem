using WorkScheduler.Models.Base;
using WorkScheduler.Models.Enums;
using WorkScheduler.Models.Shared;

namespace WorkScheduler.Models.Scheduler
{
    public class TicketFile : EntityBase<long>
    {
        public long TicketId { get; set; }
        public Ticket Ticket { get; set; }

        public long FileId { get; set; }
        public File File { get; set; }

        public TicketFileType Type { get; set; }
    }
}
