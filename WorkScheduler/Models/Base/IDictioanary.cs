using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkScheduler.Models.Base
{
    public interface IDictionary<TKey> : IEntityBase<TKey>
    {
        string Name { get; set; }
    }
}
