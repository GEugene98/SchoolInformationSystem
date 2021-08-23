using WorkScheduler.Models.Base;
using WorkScheduler.Models.Enums;
using WorkScheduler.Models.Shared;

namespace WorkScheduler.Models.Workflow
{
    public class OutgoingDocumentFile : EntityBase<long>
    {
        public int OutgoingDocumentId { get; set; }
        public OutgoingDocument OutgoingDocument { get; set; }

        public long FileId { get; set; }
        public File File { get; set; }
    }
}
