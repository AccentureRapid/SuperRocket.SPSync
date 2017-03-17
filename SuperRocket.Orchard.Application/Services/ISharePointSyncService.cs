using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperRocket.Orchard.Services
{
    public interface ISharePointSyncService : IApplicationService
    {
        string Encrypt(string value);
        string Decrypt(string value);
        dynamic LoadConfiguration();
        dynamic GetAllFoldersFromConfig();
        dynamic GetAllFoldersTreeFromConfig();
        void EnqueueSharepointSyncJob();
        Task SyncAsync();
    }
}
