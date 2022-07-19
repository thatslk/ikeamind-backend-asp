using ikeamind_backend.Core.Interfaces;
using ikeamind_backend.Core.Models.EFModels;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ikeamind_backend.Infrastructure
{
    public partial class IkeaProductsContext : DbContext, IIkeaDbContext
    {
        public IkeaProductsContext()
        {
        }

        public IkeaProductsContext(DbContextOptions<IkeaProductsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<IkeaProductRU> IkeaRu { get; set; }
        public virtual DbSet<IkeaProductSE> IkeaSe { get; set; }
        public virtual DbSet<IkeaProductUS> IkeaUs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            { }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
