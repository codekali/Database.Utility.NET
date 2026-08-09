using System;
using System.Threading.Tasks;

namespace Codekali.Net.Persistence.UoW
{
    /// <summary>
    /// Defines a unit of work.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Saves all changes until now in this unit of work.
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Saves all changes until now in this unit of work.
        /// </summary>
        Task SaveChangesAsync();
    }
}
