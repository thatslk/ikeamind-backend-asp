using ikeamind_backend.Core.Interfaces;
using ikeamind_backend.Core.Models.EFModels.AccountModels;
using Microsoft.EntityFrameworkCore;

namespace ikeamind_backend.Infrastructure
{
    public class IkeaJwtAccountsContext : DbContext, IIkeaAccountsContext
    {
        public IkeaJwtAccountsContext()
        {
        }

        public IkeaJwtAccountsContext(DbContextOptions<IkeaJwtAccountsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            { }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });
        }
    }
}
