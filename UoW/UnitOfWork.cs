using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Codekali.Net.Persistence.UoW
{
    public abstract class UnitOfWork : IUnitOfWork
    {
        private bool disposed;

        /// <inheritdoc/>
        public abstract void SaveChanges();

        /// <inheritdoc/>
        public abstract Task SaveChangesAsync();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            disposed = true;
        }
    }

    /// <summary>
    /// Implements Unit of work for Entity Framework.
    /// </summary>
    public class UnitOfWork<TDbContext> : UnitOfWork
        where TDbContext : DbContext
    {
        private bool disposed = false;
        private readonly TDbContext dbContext;


        /// <summary>
        /// Creates a new <see cref="EfCoreUnitOfWork"/>.
        /// </summary>
        public UnitOfWork(TDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public override void SaveChanges()
        {
            dbContext.SaveChanges();
        }

        public override async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            disposed = true;

            base.Dispose(disposing);
        }
    }
}
