using ikeamind_backend.Core.Interfaces;
using ikeamind_backend.Core.Models.EFModels.AccountModels;
using Microsoft.EntityFrameworkCore;

namespace ikeamind_backend.Infrastructure
{
    public partial class IkeaMindAccountsContext : DbContext, IIkeaMindAccountsContext
    {
        public IkeaMindAccountsContext()
        {
        }

        public IkeaMindAccountsContext(DbContextOptions<IkeaMindAccountsContext> options)
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
            modelBuilder.HasAnnotation("Relational:Collation", "Russian_Russia.1251");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Password).IsRequired();

                entity.Property(e => e.Username).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
