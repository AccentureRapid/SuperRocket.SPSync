﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperRocket.Orchard.Core.SharePoint.Enums
{
    public enum FileChangeType : int
    {
        Unknown = 0,
        Created = 1,
        Deleted = 2,
        Changed = 4,
        Renamed = 8,
    }
}
