using System.Data.Entity.Migrations;
using Abp.MultiTenancy;
using Abp.Zero.EntityFramework;
using SuperRocket.Orchard.Migrations.SeedData;
using EntityFramework.DynamicFilters;

namespace SuperRocket.Orchard.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<Orchard.EntityFramework.OrchardDbContext>, IMultiTenantSeed
    {
        public AbpTenantBase Tenant { get; set; }

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Orchard";
        }

        protected override void Seed(Orchard.EntityFramework.OrchardDbContext context)
        {
            context.DisableAllFilters();

            if (Tenant == null)
            {
                //Host seed
                new InitialHostDbBuilder(context).Create();

                //Default tenant seed (in host database).
                new DefaultTenantCreator(context).Create();
                new TenantRoleAndUserBuilder(context, 1).Create();
            }
            else
            {
                //You can add seed for tenant databases and use Tenant property...
            }

            context.SaveChanges();
        }
    }
}
