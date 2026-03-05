using GenericFunctions.EFCore;
using GenericFunctions.Repository;
using GenericFunctions.UoW;
using Microsoft.EntityFrameworkCore;
using System;

namespace GenericFunctions.EFNoSQL
{
    public class NoSQLUnitOfWorkManager<TDbContext> : IUnitOfWorkManager<TDbContext>, IUnitOfWorkManager
         where TDbContext : DbContext
    {
        private readonly TDbContext dbContext;

        public NoSQLUnitOfWorkManager(TDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IUnitOfWork Begin()
        {
            return new UnitOfWork<TDbContext>(dbContext);
        }

        /// <summary>
        /// Do not use this for Mongo DB Collections. 
        /// </summary>
        /// <remarks>
        /// Use <see cref="NoSQLRepository"/>.
        /// </remarks>
        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            throw new NotImplementedException();
        }

        public INoSQLRepository<TEntity> NoSQLRepository<TEntity>() where TEntity : class
        {
            return new NoSQLRepository<TEntity>();
        }
    }
}

