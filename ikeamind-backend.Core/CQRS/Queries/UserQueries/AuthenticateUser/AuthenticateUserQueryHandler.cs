using ikeamind_backend.Core.Interfaces;
using ikeamind_backend.Core.Models.EFModels.AccountModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace ikeamind_backend.Core.CQRS.Queries.UserQueries.AuthenticateUser
{
    public class AuthenticateUserQuery : IRequest<Account>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AuthenticateUserQueryHandler
        : IRequestHandler<AuthenticateUserQuery, Account>
    {
        protected readonly IIkeaAccountsContext db;
        public AuthenticateUserQueryHandler(IIkeaAccountsContext context) 
        {
            db = context;
        }

        public async Task<Account> Handle(AuthenticateUserQuery request, CancellationToken cancellationToken)
        {
            var user = await db.Accounts.SingleOrDefaultAsync(x => x.Username == request.Username && x.Password == request.Password);
            return user;
        }
    }
}
