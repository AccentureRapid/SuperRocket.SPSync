using System.Linq;
using SuperRocket.Orchard.EntityFramework;
using SuperRocket.Orchard.MultiTenancy;

namespace SuperRocket.Orchard.Migrations.SeedData
{
    public class DefaultTenantCreator
    {
        private readonly OrchardDbContext _context;

        public DefaultTenantCreator(OrchardDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateUserAndRoles();
        }

        private void CreateUserAndRoles()
        {
            //Default tenant

            var defaultTenant = _context.Tenants.FirstOrDefault(t => t.TenancyName == Tenant.DefaultTenantName);
            if (defaultTenant == null)
            {
                _context.Tenants.Add(new Tenant {TenancyName = Tenant.DefaultTenantName, Name = Tenant.DefaultTenantName});
                _context.SaveChanges();
            }
        }
    }
}
