using SuperRocket.Orchard.Core.SharePoint;
using SuperRocket.Orchard.Core.SharePoint.Enums;
using SuperRocket.Orchard.Core.SharePoint.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperRocket.Orchard.Application.Services.Args
{
    internal class SyncServiceConflictEventArgs : EventArgs
    {
        public SyncConfiguration Configuration { get; set; }
        public MetadataItem Item { get; }
        public ItemStatus NewStatus { get; set; }

        public SyncServiceConflictEventArgs(SyncConfiguration configuration, MetadataItem item, ItemStatus status)
        {
            Configuration = configuration;
            Item = item;
            NewStatus = status;
        }
    }
}
