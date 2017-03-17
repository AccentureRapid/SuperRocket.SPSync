using Abp.BackgroundJobs;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.IO;
using Amazon.S3.Model;
using Amba.System.Extensions;
using SuperRocket.Orchard.Core.SharePoint;
using SuperRocket.Orchard.Job;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperRocket.Orchard.Services
{
    public class AwsS3Service : IAwsS3Service
    {
        private readonly IBackgroundJobManager _backgroundJobManager;
        private string _bucketName;
        private string _accessKeyID;
        private string _secretKey;
        private IAmazonS3 _client;
        public AwsS3Service(IBackgroundJobManager backgroundJobManager)
        {
            _backgroundJobManager = backgroundJobManager;

            _bucketName = ConfigurationManager.AppSettings["Amazon.AwsS3UploadJob.BucketName"];
            _accessKeyID = ConfigurationManager.AppSettings["Amazon.AwsS3UploadJob.AccessKeyID"];
            _secretKey = ConfigurationManager.AppSettings["Amazon.AwsS3UploadJob.SecretKey"];
            _client = GetClient();
        }

        public void UploadFile(string fullPath)
        {
            try
            {
                if (string.IsNullOrEmpty(_bucketName))
                    throw new Exception("Amazon.AwsS3UploadJob.BucketName is not found in config.");
                if (string.IsNullOrEmpty(_accessKeyID))
                    throw new Exception("Amazon.AwsS3UploadJob.AccessKeyID is not found in config.");
                if (string.IsNullOrEmpty(_secretKey))
                    throw new Exception("Amazon.AwsS3UploadJob.SecretKey is not found in config.");

                FileInfo file = new FileInfo(fullPath);
                var fileName = file.Name;
                var directoryName = file.DirectoryName;
                var s3Path = PathToKey(file.FullName);

                if (!CheckBucketExists(_client, _bucketName))
                {
                    _client.PutBucket(_bucketName);
                }


                var inputStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);

                SaveStream(s3Path, inputStream, _bucketName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveStream(string path, Stream inputStream, string bucketName)
        {
            path = CleanPath(path);
            var file = new S3FileInfo(_client, bucketName, path);
            var isNew = !file.Exists;
            using (var stream = file.Exists ? file.OpenWrite() : file.Create())
            {
                inputStream.CopyTo(stream);
                inputStream.Close();
            }
            if (isNew)
            {
                PublishFile(path);
            }
        }

        public void PublishFile(string path)
        {
            var key = PathToKey(path);
            Console.WriteLine("Publish key:" + key);
            _client.PutACL(new PutACLRequest
            {
                BucketName = _bucketName,
                Key = key,
                CannedACL =  S3CannedACL.AuthenticatedRead
            });
        }
        public static string CleanPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return string.Empty;
            path = path.RegexRemove(@"^[^:]+\:").Replace("/", "\\").RegexRemove(@"^[\\]+");
            return path;
        }
        private IAmazonS3 GetClient()
        {
            AWSCredentials credentials;
            credentials = new BasicAWSCredentials(_accessKeyID, _secretKey);
            var client = new AmazonS3Client(_accessKeyID, _secretKey, Amazon.RegionEndpoint.USEast1);

            return (IAmazonS3)client;
        }
        private string PathToKey(string path)
        {
            var result = path.RegexRemove(@"^[^:]+\:").Replace("\\", "/").RegexRemove(@"^[\/]+");
            return result;
        }
        private SyncConfiguration GetConfiguration()
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
        public static bool CheckBucketExists(IAmazonS3 s3, String bucketName)
        {
            List<S3Bucket> buckets = s3.ListBuckets().Buckets;
            foreach (S3Bucket bucket in buckets)
            {
                if (bucket.BucketName == bucketName)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
