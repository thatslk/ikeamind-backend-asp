using ikeamind_backend.Core.Models.EFModels;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace ikeamind_backend.Core.Interfaces
{
    public interface IIkeaDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        DbSet<IkeaProductUS> IkeaUs { get; set; }
        DbSet<IkeaProductSE> IkeaSe { get; set; }
        DbSet<IkeaProductRU> IkeaRu { get; set; }
    }
}
