using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace SuperRocket.Orchard.EntityFramework.Repositories
{
    public abstract class OrchardRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<OrchardDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected OrchardRepositoryBase(IDbContextProvider<OrchardDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class OrchardRepositoryBase<TEntity> : OrchardRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected OrchardRepositoryBase(IDbContextProvider<OrchardDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
