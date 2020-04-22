using System;

namespace WorkScheduler.ViewModels.Scheduler
{
    public class ProtocolViewModel : DictionaryViewModel<int>
    {
        public ActionViewModel Action { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string Number { get; set; }
        public string Header { get; set; }
        public string Chairman { get; set; }
        public string Secretary { get; set; }
        public string Attended { get; set; }
        public string Agenda { get; set; }
        public string Listen { get; set; }
        public string Speaked { get; set; }
        public string Decided { get; set; }
    }
}
