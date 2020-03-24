using System;
using WorkScheduler.Models.Base;
using WorkScheduler.Models.Enums;
using WorkScheduler.Models.Identity;

namespace WorkScheduler.Models.Shared
{
    public class Post : EntityBase<long>
    {
        public string Text { get; set; }

        public Color Color { get; set; }

        public DateTime CreatedAt { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
