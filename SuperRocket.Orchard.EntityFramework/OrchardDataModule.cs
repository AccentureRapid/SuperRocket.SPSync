using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using Abp.Zero.EntityFramework;
using SuperRocket.Orchard.EntityFramework;

namespace SuperRocket.Orchard
{
    [DependsOn(typeof(AbpZeroEntityFrameworkModule), typeof(OrchardCoreModule))]
    public class OrchardDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<OrchardDbContext>());

            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
