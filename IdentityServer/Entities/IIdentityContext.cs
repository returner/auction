using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.Entities
{
    public interface IIdentityContext
    {
        DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        DbSet<AdminUser> AdminUsers { get; set; }
    }

}
