using ikeamind_backend.Core.Models.EFModels.UserModels;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace ikeamind_backend.Core.Interfaces
{
    public interface IIkeaUsersContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        DbSet<Avatar> Avatars { get; set; }
        DbSet<User> Users { get; set; }
    }
}
