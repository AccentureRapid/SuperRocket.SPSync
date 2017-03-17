using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using SuperRocket.Orchard.EntityFramework;

namespace SuperRocket.Orchard.Migrator
{
    [DependsOn(typeof(OrchardDataModule))]
    public class OrchardMigratorModule : AbpModule
    {
        public override void PreInitialize()
        {
            Database.SetInitializer<OrchardDbContext>(null);

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}