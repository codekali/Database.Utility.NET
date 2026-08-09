using Microsoft.EntityFrameworkCore;

namespace Codekali.Net.Persistence.UoW
{
    public class UnitOfWorkManager<TDbContext> : IUnitOfWorkManager
         where TDbContext : DbContext
    {
        private readonly TDbContext dbContext;

        public UnitOfWorkManager(TDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IUnitOfWork Begin()
        {
            return new UnitOfWork<TDbContext>(dbContext);
        }
    }
}
