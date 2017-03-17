using SuperRocket.Orchard.EntityFramework;
using EntityFramework.DynamicFilters;

namespace SuperRocket.Orchard.Migrations.SeedData
{
    public class InitialHostDbBuilder
    {
        private readonly OrchardDbContext _context;

        public InitialHostDbBuilder(OrchardDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            _context.DisableAllFilters();

            new DefaultEditionsCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();
        }
    }
}
