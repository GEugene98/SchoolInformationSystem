using WorkScheduler.Models.Base;
using WorkScheduler.Models.Enums;
using WorkScheduler.Models.Shared;

namespace WorkScheduler.Models.Workflow
{
    public class IncomingDocumentFile : EntityBase<long>
    {
        public int IncomingDocumentId { get; set; }
        public IncomingDocument IncomingDocument { get; set; }

        public long FileId { get; set; }
        public File File { get; set; }
    }
}
