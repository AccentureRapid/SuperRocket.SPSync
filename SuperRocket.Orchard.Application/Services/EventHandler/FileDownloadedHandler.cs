using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Events.Bus.Handlers;
using SuperRocket.Orchard.Job;
using SuperRocket.Orchard.Services.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperRocket.Orchard.Services.EventHandler
{
    public class FileDownloadedHandler : IEventHandler<FileDownloadedEventData>, ITransientDependency
    {
        private readonly IBackgroundJobManager _backgroundJobManager;

        public FileDownloadedHandler(IBackgroundJobManager backgroundJobManager)
        {
            _backgroundJobManager = backgroundJobManager;
        }
        public void HandleEvent(FileDownloadedEventData eventData)
        {
            Console.WriteLine(string.Format("data received : {0}  {1}  {2}", 
                eventData.FileChangeData.ChangeType, eventData.FileChangeData.FullPath,eventData.FileChangeData.OldFullPath));

            var awsS3UploadJobIsEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["Amazon.AwsS3UploadJob.IsEnabled"]);
            if (awsS3UploadJobIsEnabled)
            {
                if (eventData.FileChangeData.ChangeType == Core.SharePoint.Enums.FileChangeType.Created)
                {
                    _backgroundJobManager.Enqueue<AwsS3UploadJob, FileDownloadedEventData>(eventData);
                }
            }
            
        }
    }
}
