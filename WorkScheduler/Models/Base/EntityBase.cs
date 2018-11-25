using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkScheduler.Models.Base
{
    public abstract class EntityBase<TKey> : EntityBase, IEntityBase<TKey>
    {
        public EntityBase() : base()
        {
        }

        public virtual TKey Id { get; set; }
    }
    public abstract class EntityBase : IEntityBase
    {
        public EntityBase()
        {
        }
    }
}
