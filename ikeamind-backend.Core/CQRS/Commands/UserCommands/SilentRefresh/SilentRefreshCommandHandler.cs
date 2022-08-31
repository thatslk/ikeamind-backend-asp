using ikeamind_backend.Core.CQRS.Commands.UserCommands.SetUserTokens;
using ikeamind_backend.Core.Interfaces;
using ikeamind_backend.Core.Models.ReturnModels;
using ikeamind_backend.Core.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ikeamind_backend.Core.CQRS.Commands.UserCommands.SilentRefresh
{
    public class SilentRefreshCommand : IRequest<TokensRM>
    {
        public Guid UserId { get; set; }
        public string RefreshToken { get; set; }
    }

    public class SilentRefreshCommandHandler
        : IRequestHandler<SilentRefreshCommand, TokensRM>
    {
        private readonly IIkeaMindAccountsContext accountsDb;
        private readonly TokenGenerators _tokenGenerators;
        private readonly IMediator _mediator;

        public SilentRefreshCommandHandler
            (IIkeaMindAccountsContext accountsContext,
            TokenGenerators tokenGenerators,
            IMediator mediator)
        {
            accountsDb = accountsContext;
            _tokenGenerators = tokenGenerators;
            _mediator = mediator;
        }

        public async Task<TokensRM> Handle(SilentRefreshCommand request, CancellationToken cancellationToken)
        {
            var account = await accountsDb.Accounts.SingleOrDefaultAsync(x => x.Id == request.UserId);

            var refreshTokenExpiresDate = LinuxTimestampConverter.LinuxTimestampToDateTime(account.RefreshTokenExpires);

            if ((account.RefreshToken != request.RefreshToken) || (refreshTokenExpiresDate <= DateTime.Today))
            {
                return null;
            }

            return await _mediator.Send(new SetUserTokensCommand { UserId = request.UserId });

        }

    }
}
