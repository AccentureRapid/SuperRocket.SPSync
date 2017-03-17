using Abp.Events.Bus;
using SuperRocket.Orchard.Services.Args;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperRocket.Orchard.Services.Events
{
    public class FileDownloadedEventData : EventData
    {
        public FileChange FileChangeData { get; set; }
    }
}
