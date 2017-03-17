using SuperRocket.Orchard.Core.SharePoint.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperRocket.Orchard.Services.Args
{
    public class FileChange
    {
        public FileChange(string fullPath, WatcherChangeTypes changeType, string oldFullPath = null)
        {
            FullPath = fullPath;
            OldFullPath = oldFullPath;
            switch (changeType)
            {
                case WatcherChangeTypes.Created:
                    ChangeType = FileChangeType.Created;
                    break;
                case WatcherChangeTypes.Deleted:
                    ChangeType = FileChangeType.Deleted;
                    break;
                case WatcherChangeTypes.Changed:
                    ChangeType = FileChangeType.Changed;
                    break;
                case WatcherChangeTypes.Renamed:
                    ChangeType = FileChangeType.Renamed;
                    break;
                default:
                    ChangeType = FileChangeType.Unknown;
                    break;
            }
        }

        public string FullPath { get; set; }
        public string OldFullPath { get; set; }
        public FileChangeType ChangeType { get; set; }
    }
}
