using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkScheduler.ViewModels
{
    public class Response<BodyType>
    {
        public BodyType Body { get; set; }
        public int PageCount { get; set; }
        public int TotalItemCount { get; set; }
    }
}
