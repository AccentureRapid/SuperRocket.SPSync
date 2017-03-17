using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;
using Abp.Hangfire;
using Abp.Hangfire.Configuration;
using Hangfire;
using Hangfire.SqlServer;
using System;
using SuperRocket.Orchard.Services;

namespace SuperRocket.Orchard
{
    [DependsOn(typeof(OrchardCoreModule), typeof(AbpAutoMapperModule), typeof(AbpHangfireModule))]
    public class OrchardApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.AbpAutoMapper().Configurators.Add(mapper =>
            {
                //Add your custom AutoMapper mappings here...
                //mapper.CreateMap<,>()
            });

            Configuration.BackgroundJobs.UseHangfire(configuration =>
            {
                configuration.GlobalConfiguration.UseSqlServerStorage("Default");
            });

        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
