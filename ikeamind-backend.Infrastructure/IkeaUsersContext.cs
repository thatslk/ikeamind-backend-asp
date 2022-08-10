using ikeamind_backend.Core.Interfaces;
using ikeamind_backend.Core.Models.EFModels.UserModels;
using Microsoft.EntityFrameworkCore;

namespace ikeamind_backend.Infrastructure
{
    public partial class IkeaUsersContext : DbContext, IIkeaUsersContext
    {
        public IkeaUsersContext()
        {
        }

        public IkeaUsersContext(DbContextOptions<IkeaUsersContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Avatar> Avatars { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            { }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Avatar>(entity =>
            {
                entity.HasIndex(e => e.Id)
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Avatar)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.AvatarId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
        }
    }
}
