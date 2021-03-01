using System;
namespace WorkScheduler.ViewModels.Register
{
    public class RegisterRecordViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime? Date { get; set; }
        public int PlaningRecordId { get; set; }
    }
}
