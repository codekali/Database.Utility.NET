using GenericFunctions.Repository;
using GenericFunctions.UoW;
using Microsoft.EntityFrameworkCore;

namespace GenericFunctions.EFCore
{
    public class EFCoreUnitOfWorkManager<TDbContext> : IUnitOfWorkManager<TDbContext>, IUnitOfWorkManager
        where TDbContext : DbContext
    {
        private readonly TDbContext dbContext;

        public EFCoreUnitOfWorkManager(TDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IUnitOfWork Begin()
        {
            return new EFCoreUnitOfWork<TDbContext>(dbContext);
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            return new EFCoreRepository<TDbContext, TEntity>(new DefaultDbContextProvider<TDbContext>(dbContext));
        }
    }
}
