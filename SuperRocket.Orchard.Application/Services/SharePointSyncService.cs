using Abp.BackgroundJobs;
using SuperRocket.Orchard.Application.Services.Args;
using SuperRocket.Orchard.Application.Services.Enums;
using SuperRocket.Orchard.Core.SharePoint;
using SuperRocket.Orchard.Core.SharePoint.Common;
using SuperRocket.Orchard.Core.SharePoint.Enums;
using SuperRocket.Orchard.Core.SharePoint.Metadata;
using SuperRocket.Orchard.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SuperRocket.Orchard.Services
{
    public class SharePointSyncService : ISharePointSyncService
    {
        private readonly IBackgroundJobManager _backgroundJobManager;

        private Dictionary<string, SyncServiceFileWatcher> _watchers = new Dictionary<string, SyncServiceFileWatcher>();
        private Dictionary<string, SyncManager> _syncManagers = new Dictionary<string, SyncManager>();
        private Dictionary<string, bool> _initialSyncCache = new Dictionary<string, bool>();
        public SharePointSyncService(IBackgroundJobManager backgroundJobManager)
        {
            _backgroundJobManager = backgroundJobManager;
        }

        public string Encrypt(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            var data = Encoding.Default.GetBytes(value);
            //var result = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(data);
        }

        public string Decrypt(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            var data = Convert.FromBase64String(value);
            //var result = ProtectedData.Unprotect(data, null, DataProtectionScope.CurrentUser);
            return Encoding.Default.GetString(data);
        }
        public dynamic LoadConfiguration()
        {
            SyncConfiguration config = GetConfiguration();
            var result = new
            {
                config.Name,
                config.SiteUrl,
                config.DocumentLibrary,
                config.LocalFolder,
                config.DownloadHeadersOnly,
                config.ConflictHandling,
                config.Direction,
                config.PasswordEncrypted,
                config.Username
            };
            return result;
        }
        public dynamic GetAllFoldersFromConfig()
        {
            SyncConfiguration config = GetConfiguration();
            var sharePointManager = config.GetSharePointManager();
            var result = SharePointManager.GetAllFoldersFromConfig(config);
            return result;
        }

        public dynamic GetAllFoldersTreeFromConfig()
        {
            SyncConfiguration config = GetConfiguration();
            var sharePointManager = config.GetSharePointManager();
            var result = SharePointManager.GetAllFoldersTreeFromConfig(config);
            return result;
        }

        public void EnqueueSharepointSyncJob()
        {
            _backgroundJobManager.Enqueue<SharePointSyncJob, int>(1);
        }
        public async Task SyncAsync()
        {
            SyncConfiguration config = GetConfiguration();

            var manager = GetSyncManager(config);

            var rescanLocalFiles = true;

            if (_initialSyncCache.ContainsKey(config.LocalFolder))
                rescanLocalFiles = _initialSyncCache[config.LocalFolder];

            await manager.SynchronizeAsync(rescanLocalFiles: rescanLocalFiles);

            _initialSyncCache[config.LocalFolder] = false;
        }

        internal int Sync(SyncConfiguration conf)
        {
            var manager = GetSyncManager(conf);

            var rescanLocalFiles = true;
            if (_initialSyncCache.ContainsKey(conf.LocalFolder))
                rescanLocalFiles = _initialSyncCache[conf.LocalFolder];

            var changeCount = manager.Synchronize(rescanLocalFiles: rescanLocalFiles);

            _initialSyncCache[conf.LocalFolder] = false;

            return changeCount;
        }

        internal void SyncLocalFileChange(SyncConfiguration conf, string fullPath, FileChangeType changeType, string oldFullPath = null)
        {
            var manager = GetSyncManager(conf);

            manager.SynchronizeLocalFileChange(fullPath, changeType, oldFullPath);
        }
        private static SyncConfiguration GetConfiguration()
        {
            SyncConfiguration.RevertConfigurationChanges();
            var configPair = SyncConfiguration.AllConfigurations.FirstOrDefault();
            if (configPair.Value == null)
            {
                throw new Exception("No Configuration found for sharepoint sync process.");
            }
            var config = configPair.Value;
            return config;
        }

        private SyncManager GetSyncManager(SyncConfiguration conf)
        {
            if (!_syncManagers.ContainsKey(conf.LocalFolder))
            {
                lock (_syncManagers)
                {
                    if (!_syncManagers.ContainsKey(conf.LocalFolder))
                    {
                        SyncManager manager = new SyncManager(conf.LocalFolder);
                        manager.SyncProgress += new EventHandler<SyncProgressEventArgs>(manager_SyncProgress);
                        manager.ItemProgress += new EventHandler<ItemProgressEventArgs>(manager_ItemProgress);
                        manager.ItemConflict += new EventHandler<ConflictEventArgs>(manager_ItemConflict);
                        _syncManagers.Add(conf.LocalFolder, manager);
                    }
                }
            }

            return _syncManagers[conf.LocalFolder];
        }

        private void manager_ItemProgress(object sender, ItemProgressEventArgs e)
        {
            OnProgress(e.Configuration, e.ItemType, e.Status, e.Percent, e.Message, e.InnerException);

            Logger.Log("[{4}] [{5}] {2} Item ({3}): {1}% - {0}", e.Message, e.Percent, e.Status, e.ItemType, DateTime.Now, e.Configuration.Name);
            if (e.InnerException != null)
                Logger.Log("[{0}] [{1}] {2}{3}{4}", DateTime.Now, e.Configuration.Name, e.InnerException.Message, Environment.NewLine, e.InnerException.StackTrace);
        }

        private void manager_SyncProgress(object sender, SyncProgressEventArgs e)
        {
            OnProgress(e.Configuration, e.Status, e.Percent, e.Message, e.InnerException);

            Logger.Log("[{3}] [{4}] {2} Sync: {1}% - {0}", e.Message, e.Percent, e.Status, DateTime.Now, e.Configuration.Name);
            if (e.InnerException != null)
            {
                var ie = e.InnerException;
                while (ie != null)
                {
                    Logger.Log("[{0}] [{1}] {2}{3}{4}", DateTime.Now, e.Configuration.Name, ie.Message, Environment.NewLine, ie.StackTrace);
                    ie = ie.InnerException;
                }
            }
        }
        private void manager_ItemConflict(object sender, ConflictEventArgs e)
        {
            e.NewStatus = OnConflict(e.Configuration, e.Item, e.NewStatus);

            Logger.Log("[{0}] [{3}] Conflict {1} - New Status: {2}", DateTime.Now, e.Item.Name, e.NewStatus, e.Configuration.Name);
        }


        #region Events

        internal event EventHandler<SyncServiceProgressEventArgs> Progress;
        internal event EventHandler<SyncServiceConflictEventArgs> Conflict;

        private void OnProgress(SyncConfiguration configuration, ItemType type, ProgressStatus status, int percent, string message, Exception innerException = null)
        {
            if (Progress != null)
            {
                SyncEventType t = SyncEventType.Unknown;
                if (type == ItemType.File)
                    t = SyncEventType.File;
                if (type == ItemType.Folder)
                    t = SyncEventType.Folder;
                Progress(this, new SyncServiceProgressEventArgs(configuration, t, status, percent, message, innerException));
            }
        }

        private void OnProgress(SyncConfiguration configuration, ProgressStatus status, int percent, string message, Exception innerException = null)
        {
            if (Progress != null)
                Progress(this, new SyncServiceProgressEventArgs(configuration, SyncEventType.Overall, status, percent, message, innerException));
        }

        private ItemStatus OnConflict(SyncConfiguration configuration, MetadataItem item, ItemStatus status)
        {
            var stat = status;
            if (Conflict != null)
            {
                var args = new SyncServiceConflictEventArgs(configuration, item, status);
                Conflict(this, args);
                stat = args.NewStatus;
            }
            return stat;
        }

        #endregion

    }
}
