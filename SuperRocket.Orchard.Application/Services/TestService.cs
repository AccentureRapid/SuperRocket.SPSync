using Abp.BackgroundJobs;
using SuperRocket.Orchard.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperRocket.Orchard.Services
{
    public class TestService : ITestService
    {
        private readonly IBackgroundJobManager _backgroundJobManager;

        public TestService(IBackgroundJobManager backgroundJobManager)
        {
            _backgroundJobManager = backgroundJobManager;
        }

        public void Test()
        {
            _backgroundJobManager.Enqueue<TestJob, int>(1);
        }
    }
}
