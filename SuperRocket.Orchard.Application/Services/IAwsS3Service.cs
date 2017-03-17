using Abp.Application.Services;
using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperRocket.Orchard.Services
{
    public interface IAwsS3Service : ITransientDependency
    {
        void UploadFile(string fullPath);
    }
}
