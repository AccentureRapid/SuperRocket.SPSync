using System.Data.Common;
using Abp.Zero.EntityFramework;
using SuperRocket.Orchard.Authorization.Roles;
using SuperRocket.Orchard.MultiTenancy;
using SuperRocket.Orchard.Users;

namespace SuperRocket.Orchard.EntityFramework
{
    public class OrchardDbContext : AbpZeroDbContext<Tenant, Role, User>
    {
        //TODO: Define an IDbSet for your Entities...

        /* NOTE: 
         *   Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         *   But it may cause problems when working Migrate.exe of EF. If you will apply migrations on command line, do not
         *   pass connection string name to base classes. ABP works either way.
         */
        public OrchardDbContext()
            : base("Default")
        {

        }

        /* NOTE:
         *   This constructor is used by ABP to pass connection string defined in OrchardDataModule.PreInitialize.
         *   Notice that, actually you will not directly create an instance of OrchardDbContext since ABP automatically handles it.
         */
        public OrchardDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        //This constructor is used in tests
        public OrchardDbContext(DbConnection connection)
            : base(connection, true)
        {

        }
    }
}
