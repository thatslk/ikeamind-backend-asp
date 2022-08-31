using ikeamind_backend.Core.Interfaces;
using ikeamind_backend.Core.Models.ReturnModels;
using ikeamind_backend.Core.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ikeamind_backend.Core.CQRS.Commands.UserCommands.SetUserTokens
{
    public class SetUserTokensCommand : IRequest<TokensRM>
    {
        public Guid UserId { get; set; }
    }

    public class SetUserTokensCommandHandler
        : IRequestHandler<SetUserTokensCommand, TokensRM>
    {

        protected readonly IIkeaMindAccountsContext accountsDb;
        protected readonly TokenGenerators _tokenGenerators;
        public SetUserTokensCommandHandler
            (IIkeaMindAccountsContext accountsContext, 
            TokenGenerators tokenGenerators)
        {
            accountsDb = accountsContext;
            _tokenGenerators = tokenGenerators;
        }

        public async Task<TokensRM> Handle(SetUserTokensCommand request, CancellationToken cancellationToken)
        {
            var account = await accountsDb.Accounts.SingleOrDefaultAsync(x => x.Id == request.UserId);
            if(!string.IsNullOrEmpty(account.RefreshToken))
            {
                var refreshTokenExpiresDate = LinuxTimestampConverter.LinuxTimestampToDateTime(account.RefreshTokenExpires);
                if ((refreshTokenExpiresDate > DateTime.Today))
                {
                    var existedTokens = new TokensRM
                    {
                        AccessToken = _tokenGenerators.GenerateJWT(account.Username, account.Id.ToString()),
                        RefreshTokenData = new RefreshTokenDTO
                        {
                            RefreshToken = account.RefreshToken,
                            ExpiresDate = account.RefreshTokenExpires
                        }
                    };

                    return existedTokens;
                }
            }

            var expiredDay = DateTime.Today.AddDays(31);

            var refreshTokenData = new RefreshTokenDTO
            {
                RefreshToken = _tokenGenerators.GenerateRefreshToken(),
                ExpiresDate = ((DateTimeOffset)expiredDay).ToUnixTimeSeconds().ToString()
            };

            var toReturn = new TokensRM
            {
                AccessToken = _tokenGenerators.GenerateJWT(account.Username, account.Id.ToString()),
                RefreshTokenData = refreshTokenData
            };

            account.RefreshToken = refreshTokenData.RefreshToken;
            account.RefreshTokenCreated = DateTime.Today.ToString("yyyy-MM-dd");
            account.RefreshTokenExpires = ((DateTimeOffset)expiredDay).ToUnixTimeSeconds().ToString();

            await accountsDb.SaveChangesAsync();

            return toReturn;
        }

        
    }
}
