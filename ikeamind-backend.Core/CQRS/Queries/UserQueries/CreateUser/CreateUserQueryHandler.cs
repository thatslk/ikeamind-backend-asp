using ikeamind_backend.Core.Interfaces;
using ikeamind_backend.Core.Models.EFModels.AccountModels;
using ikeamind_backend.Core.Models.EFModels.UserModels;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ikeamind_backend.Core.CQRS.Queries.UserQueries.CreateUser
{
    public class CreateUserQuery : IRequest<Account>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class CreateUserQueryHandler
        : IRequestHandler<CreateUserQuery, Account>
    {
        protected readonly IIkeaProductsAndUsersContext usersDb;
        protected readonly IIkeaMindAccountsContext accountsDb;
        public CreateUserQueryHandler
            (IIkeaProductsAndUsersContext userContext,
            IIkeaMindAccountsContext accountsContext)
        {
            usersDb = userContext;
            accountsDb = accountsContext;
        }

        public async Task<Account> Handle(CreateUserQuery request, CancellationToken cancellationToken)
        {
            var newUserId = Guid.NewGuid();
            var accountToAdd = new Account
            {
                Id = newUserId,
                Username = request.Username,
                Password = request.Password
            };

            await accountsDb.Accounts.AddAsync(accountToAdd);
            await accountsDb.SaveChangesAsync();

            var userToAdd = new User
            {
                Id = newUserId,
                Username = request.Username,
                AvatarId = 1,
                BestscoreTf = 0,
                BestscorePf = 0
            };

            await usersDb.Users.AddAsync(userToAdd);
            await usersDb.SaveChangesAsync();

            return accountToAdd;
        }
    }
}
