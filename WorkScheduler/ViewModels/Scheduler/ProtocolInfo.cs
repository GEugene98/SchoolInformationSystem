using System;
namespace WorkScheduler.ViewModels.Scheduler
{
    public class ProtocolInfo
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ActionName { get; set; }
        public DateTime ActionDate { get; set; }
        public int ActionId { get; set; }
        public string ScheduleOwner { get; set; }
        public int ScheduleId { get; set; }
    }
}
