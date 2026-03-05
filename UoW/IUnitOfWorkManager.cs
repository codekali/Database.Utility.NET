using GenericFunctions.Repository;
using Microsoft.EntityFrameworkCore;

namespace GenericFunctions.UoW
{
    public interface IUnitOfWorkManager
    {
        /// <summary>
        /// Begins a new unit of work.
        /// </summary>
        /// <returns>A handle to be able to complete the unit of work</returns>
        IUnitOfWork Begin();
    }

    public interface IUnitOfWorkManager<TContext> : IUnitOfWorkManager where TContext : DbContext
    {
        /// <summary>
        /// Get the instance of generic repository
        /// </summary>
        /// <typeparam name="TEntity">Entity</typeparam>
        /// <returns></returns>
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;
    }
}
