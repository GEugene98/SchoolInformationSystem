using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkScheduler.Models.Base;
using WorkScheduler.Models.Identity;

namespace WorkScheduler.Models.Shared
{
    public class School : Dictionary<int>
    {
        public string ShortName { get; set; }
        public string Email { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public string DocumentHeaderHTML { get; set; }
        public string ActionNamesToMakeProtocolJSON { get; set; }
    }
}
