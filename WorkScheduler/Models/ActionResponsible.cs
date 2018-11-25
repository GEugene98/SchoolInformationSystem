using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkScheduler.Models.Identity;

namespace WorkScheduler.Models
{
    public class ActionUser
    {
        public int ActionId { get; set; }
        public Action Action { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
