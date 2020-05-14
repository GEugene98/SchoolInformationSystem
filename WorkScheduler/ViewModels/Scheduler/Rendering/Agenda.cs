using System;
using System.Collections.Generic;

namespace WorkScheduler.ViewModels.Scheduler.Rendering
{
    public class Agenda
    {
        public string Content { get; set; }
        public UserViewModel Author { get; set; }
        public List<InnerContent> Listen { get; set; }
        public List<InnerContent> Speaked { get; set; }
        public string Decided { get; set; }
    }

    public class InnerContent
    {
        public string Content { get; set; }
        public UserViewModel User { get; set; }
    }
}