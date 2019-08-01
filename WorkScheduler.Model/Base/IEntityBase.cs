using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkScheduler.Models.Base
{
    public interface IEntityBase
    {
    }

    public interface IEntityBase<TKey> : IEntityBase
    {
        TKey Id { get; set; }
    }
}
