using Microsoft.EntityFrameworkCore;

namespace GenericFunctions.EFCore
{
    public sealed class DefaultDbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
         where TDbContext : DbContext
    {
        public TDbContext DbContext { get; }

        public DefaultDbContextProvider(TDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public TDbContext GetDbContext()
        {
            return DbContext;
        }

        public void Dispose()
        {
            DbContext?.Dispose();
        }
    }
}
