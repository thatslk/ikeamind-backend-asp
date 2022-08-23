using ikeamind_backend.Core.Models.EFModels;
using ikeamind_backend.Core.Models.EFModels.UserModels;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace ikeamind_backend.Core.Interfaces
{
    public interface IIkeaProductsAndUsersContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        DbSet<Avatar> Avatars { get; set; }
        DbSet<AvatarDescriptionByLocale> AvatarDescriptionByLocales { get; set; }
        DbSet<IkeaProductRU> IkeaRus { get; set; }
        DbSet<IkeaProductSE> IkeaSes { get; set; }
        DbSet<IkeaProductUS> IkeaUs { get; set; }
        DbSet<User> Users { get; set; }

    }
}
