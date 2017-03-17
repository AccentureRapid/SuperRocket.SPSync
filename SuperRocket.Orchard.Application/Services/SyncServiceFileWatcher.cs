using Abp.Application.Services;
using Abp.Events.Bus;
using SuperRocket.Orchard.Core.SharePoint;
using SuperRocket.Orchard.Core.SharePoint.Enums;
using SuperRocket.Orchard.Core.SharePoint.Metadata;
using SuperRocket.Orchard.Services.Args;
using SuperRocket.Orchard.Services.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SuperRocket.Orchard.Services
{
    internal class SyncServiceFileWatcher : ApplicationService , IDisposable
    {
        private SharePointSyncService _service;
        private SyncConfiguration _config;
        private Queue<FileChange> _fileChangeQueue = new Queue<FileChange>();
        private System.Timers.Timer _fileChangeDetectionTimer;
        private FileSystemWatcher _fs;

        public SyncServiceFileWatcher(SharePointSyncService service, SyncConfiguration syncConfig)
        {
            _service = service;
            _config = syncConfig;
            _fileChangeDetectionTimer = new System.Timers.Timer(5000);
            _fileChangeDetectionTimer.Elapsed += DetectChangeTimer;
            Init();
            CurrentEventBus = NullEventBus.Instance;
        }

        public IEventBus CurrentEventBus { get; set; }
        public void Dispose()
        {
            if (_fs != null)
                _fs.Dispose();
        }

        private void Init()
        {
            try
            {
                _fs = new FileSystemWatcher(_config.LocalFolder);
                _fs.IncludeSubdirectories = true;
                _fs.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Size;
                _fs.Changed += new FileSystemEventHandler(fs_Changed);
                _fs.Created += new FileSystemEventHandler(fs_Changed);
                _fs.Deleted += new FileSystemEventHandler(fs_Changed);
                _fs.Renamed += new RenamedEventHandler(fs_Renamed);
                _fs.EnableRaisingEvents = true;
            }
            catch
            { }

        }

        private void fs_Renamed(object sender, RenamedEventArgs e)
        {
            DetectChange(new FileChange(e.FullPath, e.ChangeType, e.OldFullPath));
        }

        private void fs_Changed(object sender, FileSystemEventArgs e)
        {
            DetectChange(new FileChange(e.FullPath, e.ChangeType));
        }

        private void DetectChange(FileChange change)
        {
            if (change.FullPath.EndsWith(".spsync"))
                return;

            if (Directory.GetParent(change.FullPath).Name == MetadataStore.STOREFOLDER)
                return;

            lock (_fileChangeQueue)
            {
                if (_fileChangeQueue.Count > 0)
                {
                    var prevChange = _fileChangeQueue.Peek();
                    if (prevChange.FullPath == change.FullPath && prevChange.ChangeType == change.ChangeType)
                        return;
                }
                _fileChangeQueue.Enqueue(change);

                if (!_fileChangeDetectionTimer.Enabled)
                {
                    _fileChangeDetectionTimer.Interval = 5000;
                    _fileChangeDetectionTimer.Start();
                }
            }
        }

        private void DetectChangeTimer(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (_fileChangeQueue)
            {
                // already stopped on last access within the lock
                if (!_fileChangeDetectionTimer.Enabled)
                    return;

                if (_fileChangeQueue.Count < 1)
                {
                    _fileChangeDetectionTimer.Stop();
                    _service.Sync(_config);
                    return;
                }

                var change = _fileChangeQueue.Dequeue();

                //TODO broadcast a event to be handled
                //This will be injected and resolve this type through container
                CurrentEventBus.TriggerAsync(new FileDownloadedEventData { FileChangeData = change });
                //This is the default bus for can not inject
                EventBus.Default.Trigger(new FileDownloadedEventData { FileChangeData = change });

                _service.SyncLocalFileChange(_config, change.FullPath, change.ChangeType, change.OldFullPath);

                _fileChangeDetectionTimer.Interval = 5000;

                if (_fileChangeQueue.Count < 1)
                    _fileChangeDetectionTimer.Stop();
            }
        }
    }
}
