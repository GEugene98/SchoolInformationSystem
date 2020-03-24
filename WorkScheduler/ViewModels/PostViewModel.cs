using System;

namespace WorkScheduler.ViewModels
{
    public class PostViewModel
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public string Color { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public UserViewModel Author { get; set; }
    }
}
