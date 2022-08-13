using ikeamind_backend.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ikeamind_backend.Core.CQRS.Commands.UserCommands.SetUserName
{
    public class SetUserNameCommand : IRequest<bool>
    {
        public Guid UserId { get; set; }
        public string NewUserName { get; set; }
    }

    public class SetUserNameCommandHandler
        : IRequestHandler<SetUserNameCommand, bool>
    {
        protected readonly IIkeaUsersContext usersDb;
        protected readonly IIkeaAccountsContext accountsDb;
        public SetUserNameCommandHandler
            (IIkeaUsersContext userContext,
            IIkeaAccountsContext accountsContext)
        {
            usersDb = userContext;
            accountsDb = accountsContext;
        }

        public async Task<bool> Handle(SetUserNameCommand request, CancellationToken cancellationToken)
        {
            if (request.NewUserName.Contains(' '))
            {
                return false;
            }

            var account = await accountsDb.Accounts.SingleOrDefaultAsync(x => x.Id == request.UserId.ToString());
            var user = await usersDb.Users.SingleOrDefaultAsync(x => x.Id == request.UserId.ToString());

            user.Username = request.NewUserName;
            account.Username = request.NewUserName;

            await accountsDb.SaveChangesAsync();
            await usersDb.SaveChangesAsync();

            return true;
        }
    }
}
