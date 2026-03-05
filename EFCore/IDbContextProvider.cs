using Microsoft.EntityFrameworkCore;

namespace GenericFunctions.EFCore
{
    public interface IDbContextProvider<out TDbContext>
         where TDbContext : DbContext
    {
        TDbContext GetDbContext();
    }
}
