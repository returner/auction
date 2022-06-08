using Microsoft.EntityFrameworkCore;

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

}
