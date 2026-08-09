using Codekali.Net.Persistence.EFNoSQL;
using Codekali.Net.Persistence.Repository;
using Codekali.Net.Persistence.UoW;
using Microsoft.EntityFrameworkCore;
using System;

namespace Codekali.Net.Persistence.EFCore
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

        /// <summary>
        /// Do not use this for SQL Database Tables. 
        /// </summary>
        /// <remarks>
        /// Use <see cref="Repository"/>.
        /// </remarks>
        public INoSQLRepository<TEntity> NoSQLRepository<TEntity>() where TEntity : class
        {
            throw new NotImplementedException();
        }
    }
}
