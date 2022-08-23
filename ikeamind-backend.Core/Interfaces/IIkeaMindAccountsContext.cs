using ikeamind_backend.Core.Models.EFModels.AccountModels;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace ikeamind_backend.Core.Interfaces
{
    public interface IIkeaMindAccountsContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        DbSet<Account> Accounts { get; set; }
    }
}
