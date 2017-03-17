using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperRocket.Orchard.Core.SharePoint.Enums
{
    public enum ConflictHandling : int
    {
        ManualConflictHandling = 0,
        OverwriteLocalChanges = 1,
        OverwriteRemoteChanges = 2,
    }
}
