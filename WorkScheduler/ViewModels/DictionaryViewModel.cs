using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkScheduler.ViewModels
{
    public class DictionaryViewModel<TKey>
    {
        public TKey Id { get; set; }
        public string Name { get; set; }
    }
}
