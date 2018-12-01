using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkScheduler.Models.Base;

namespace WorkScheduler.Models
{
    public class ConfirmationForm : Dictionary<int>
    {
        public virtual ICollection<Action> Actions { get; set; }
    }
}
