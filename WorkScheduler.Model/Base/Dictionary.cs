using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkScheduler.Models.Base
{
    public abstract class Dictionary<TKey> : EntityBase<TKey>, IDictionary<TKey>
    {
        public string Name { get; set; }
    }
}
