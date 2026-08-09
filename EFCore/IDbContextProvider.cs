using Microsoft.EntityFrameworkCore;

namespace Codekali.Net.Persistence.EFCore
{
    public interface IDbContextProvider<out TDbContext>
         where TDbContext : DbContext
    {
        TDbContext GetDbContext();
    }
}
