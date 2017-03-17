using Abp.BackgroundJobs;
using Abp.Dependency;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using SuperRocket.Orchard.Services;
using SuperRocket.Orchard.Services.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SuperRocket.Orchard.Job
{
    public class AwsS3UploadJob : BackgroundJob<FileDownloadedEventData>, ITransientDependency
    {
        private readonly IAwsS3Service _awsS3Service;

        public AwsS3UploadJob(IAwsS3Service awsS3Service)
        {
            _awsS3Service = awsS3Service;
        }
        public override void Execute(FileDownloadedEventData data)
        {
            _awsS3Service.UploadFile(data.FileChangeData.FullPath);
        }
    }
}
