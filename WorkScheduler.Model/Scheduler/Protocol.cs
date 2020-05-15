using System;
using WorkScheduler.Models.Base;

namespace WorkScheduler.Models
{
    public class Protocol : Dictionary<int>
    {
        public int ActionId { get; set; }
        public Action Action { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string Number { get; set; }
        public string Header { get; set; }
        public string Chairman { get; set; }
        public string Secretary { get; set; }
        public string Attended { get; set; }
        public string ProtocolContentJSON { get; set; }
    }
}
