using ikeamind_backend.Core.Interfaces;
using ikeamind_backend.Core.Models.EFModels;
using ikeamind_backend.Core.Models.EFModels.UserModels;
using Microsoft.EntityFrameworkCore;

namespace ikeamind_backend.Infrastructure
{
    public partial class IkeaProductsAndUsersContext : DbContext, IIkeaProductsAndUsersContext
    {
        public IkeaProductsAndUsersContext()
        { }

        public IkeaProductsAndUsersContext(DbContextOptions<IkeaProductsAndUsersContext> options)
            : base(options)
        { }

        public virtual DbSet<Avatar> Avatars { get; set; }
        public virtual DbSet<AvatarDescriptionByLocale> AvatarDescriptionByLocales { get; set; }
        public virtual DbSet<IkeaProductRU> IkeaRus { get; set; }
        public virtual DbSet<IkeaProductSE> IkeaSes { get; set; }
        public virtual DbSet<IkeaProductUS> IkeaUs { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            { }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Russian_Russia.1251");

            modelBuilder.Entity<Avatar>(entity =>
            {
                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.Property(e => e.ImageUrl).HasColumnName("ImageURL");
            });

            modelBuilder.Entity<AvatarDescriptionByLocale>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.AvatarId)
                    .ValueGeneratedOnAdd()
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.LocaleCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.HasOne(d => d.Avatar)
                    .WithMany()
                    .HasForeignKey(d => d.AvatarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_avatarId");
            });

            modelBuilder.Entity<IkeaProductRU>(entity =>
            {
                entity.ToTable("IkeaRU");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.Property(e => e.Article)
                    .HasMaxLength(8)
                    .HasColumnName("article");

                entity.Property(e => e.ArticlePrefix).HasColumnName("articlePrefix");

                entity.Property(e => e.Category).HasColumnName("category");

                entity.Property(e => e.GlobalArticle).HasColumnName("globalArticle");

                entity.Property(e => e.GlobalArticlePrefix).HasColumnName("globalArticlePrefix");

                entity.Property(e => e.ImageUrl).HasColumnName("imageUrl");

                entity.Property(e => e.Measure).HasColumnName("measure");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Url).HasColumnName("url");
            });

            modelBuilder.Entity<IkeaProductSE>(entity =>
            {
                entity.ToTable("IkeaSE");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.Property(e => e.Article)
                    .HasMaxLength(8)
                    .HasColumnName("article");

                entity.Property(e => e.ArticlePrefix).HasColumnName("articlePrefix");

                entity.Property(e => e.Category).HasColumnName("category");

                entity.Property(e => e.GlobalArticle).HasColumnName("globalArticle");

                entity.Property(e => e.GlobalArticlePrefix).HasColumnName("globalArticlePrefix");

                entity.Property(e => e.ImageUrl).HasColumnName("imageUrl");

                entity.Property(e => e.Measure).HasColumnName("measure");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Url).HasColumnName("url");
            });

            modelBuilder.Entity<IkeaProductUS>(entity =>
            {
                entity.ToTable("IkeaUS");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.Property(e => e.Article)
                    .HasMaxLength(8)
                    .HasColumnName("article");

                entity.Property(e => e.ArticlePrefix).HasColumnName("articlePrefix");

                entity.Property(e => e.Category).HasColumnName("category");

                entity.Property(e => e.GlobalArticle).HasColumnName("globalArticle");

                entity.Property(e => e.GlobalArticlePrefix).HasColumnName("globalArticlePrefix");

                entity.Property(e => e.ImageUrl).HasColumnName("imageUrl");

                entity.Property(e => e.Measure).HasColumnName("measure");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Url).HasColumnName("url");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.BestscoreTf).HasColumnName("Bestscore_TF");

                entity.Property(e => e.BestscorePf).HasColumnName("Bestscore_PF");

                entity.Property(e => e.Username).IsRequired();

                entity.HasOne(d => d.Avatar)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.AvatarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_AvatarId");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
