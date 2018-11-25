﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkScheduler.Models.Enums
{
    public enum ActionStatus
    {
        New,
        NeedConfirm,
        Confirmed,
        Accepted,
        CanceledConfirming,
        CanceledAccepting,
    }
}
