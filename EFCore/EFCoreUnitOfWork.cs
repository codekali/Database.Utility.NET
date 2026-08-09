using Codekali.Net.Persistence.UoW;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Codekali.Net.Persistence.EFCore
{
    /// <summary>
    /// Implements Unit of work for Entity Framework.
    /// </summary>
    public class EFCoreUnitOfWork<TDbContext> : UnitOfWork
        where TDbContext : DbContext
    {
        private bool disposed = false;
        private readonly TDbContext dbContext;


        /// <summary>
        /// Creates a new <see cref="EfCoreUnitOfWork"/>.
        /// </summary>
        public EFCoreUnitOfWork(TDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public override void SaveChanges()
        {
            try
            {
                dbContext.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw HandleDbException(ex);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public override async Task SaveChangesAsync()
        {
            try
            {
                await dbContext.SaveChangesAsync();

                foreach (var entry in dbContext.ChangeTracker.Entries().ToArray())
                {
                    entry.State = EntityState.Detached;
                }
            }
            catch (DbUpdateException ex)
            {
                throw HandleDbException(ex);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            disposed = true;

            base.Dispose(disposing);
        }

        private SqlException HandleDbException(DbUpdateException e)
        {
            var sqlException = e.GetBaseException() as SqlException;
            return sqlException;
        }
    }
}

