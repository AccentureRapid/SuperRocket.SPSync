using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperRocket.Orchard.Services
{
    public interface ITestService : IApplicationService
    {
        void Test();
    }
}
