using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Identity.Entities
{
    public class IdentityContext : DbContext, IIdentityContext
    {
        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<AdminUser> AdminUsers { get; set; } = null!;
    }

    public interface IIdentityContext
    {
        DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        DbSet<AdminUser> AdminUsers { get; set; }
    }

}
