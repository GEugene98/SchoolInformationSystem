using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkScheduler.Models.Enums;

namespace WorkScheduler.ViewModels
{
    public class ActivityViewModel : DictionaryViewModel<int>
    {
        public Color Color { get; set; }
    }
}
