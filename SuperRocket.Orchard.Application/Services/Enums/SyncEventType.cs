using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperRocket.Orchard.Application.Services.Enums
{
    internal enum SyncEventType : int
    {
        Unknown = 0,
        File = 1,
        Folder = 2,
        Overall = 3
    }
}
