using ikeamind_backend.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace ikeamind_backend.Core.CQRS.Queries.UserQueries.IsUsernameTaken
{
    public class IsUsernameTakenQuery : IRequest<bool>
    {
        public string Username { get; set; }
    }

    public class IsUsernameTakenQueryHandler
        : IRequestHandler<IsUsernameTakenQuery, bool>
    {
        protected readonly IIkeaAccountsContext db;
        public IsUsernameTakenQueryHandler(IIkeaAccountsContext context)
        {
            db = context;
        }

        public async Task<bool> Handle(IsUsernameTakenQuery request, CancellationToken cancellationToken)
        {
            var acoountFromDb = await db.Accounts.SingleOrDefaultAsync(x => x.Username == request.Username);
            return (acoountFromDb != null);
        }
    }
}
