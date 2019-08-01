using System;
using WorkScheduler.Models.Base;
using WorkScheduler.Models.Identity;

namespace WorkScheduler.Models.Shared
{
    public class LoginLog : EntityBase<long>
    {
        public LoginLog()
        {

        }

        public LoginLog(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; set; }
        public User User { get; set; }

        public DateTime LoggedOn { get; set; } = DateTime.Now;
    }
}
