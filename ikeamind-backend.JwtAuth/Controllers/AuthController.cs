using ikeamind_backend.Core.CQRS.Commands.UserCommands.SetUserTokens;
using ikeamind_backend.Core.CQRS.Commands.UserCommands.SilentRefresh;
using ikeamind_backend.Core.CQRS.Queries.UserQueries.AuthenticateUser;
using ikeamind_backend.Core.CQRS.Queries.UserQueries.CreateUser;
using ikeamind_backend.Core.CQRS.Queries.UserQueries.IsUsernameTaken;
using ikeamind_backend.Core.Models.EFModels.AccountModels;
using ikeamind_backend.Core.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace ikeamind_backend.JwtAuth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IOptions<AuthOptions> _authOptions;
        public AuthController(IMediator mediator, IOptions<AuthOptions> authOptions)
        {
            _mediator = mediator;
            _authOptions = authOptions;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(string username, string password)
        {
            if(username.Contains(' '))
            {
                return BadRequest("Пробелы в имени пользователя не поддерживаются");
            }

            var account = await _mediator.Send(new AuthenticateUserQuery { Username = username, Password = password });
            if (account == null)
            {
                return BadRequest("Пользователь с таким данными не найден");
            }

            var tokens = await _mediator.Send(new SetUserTokensCommand { UserId = Guid.Parse(account.Id) });
            return Ok(tokens);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Signup(string username, string password)
        {
            if (username.Contains(' '))
            {
                return BadRequest("Пробелы в имени пользователя не поддерживаются");
            }

            if (await _mediator.Send(new IsUsernameTakenQuery { Username = username }))
            {
                return BadRequest("Пользовтель с указанным логином уже существует");
            }

            var createdAccount = await _mediator.Send(new CreateUserQuery { Username = username, Password = password });

            var tokens = await _mediator.Send(new SetUserTokensCommand { UserId = Guid.Parse(createdAccount.Id) });
            return Ok(tokens);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SilentRefresh(string accessToken, string refreshToken)
        {
            if((accessToken == null) || (refreshToken == null))
            {
                return BadRequest();
            }

            var principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                return BadRequest();
            }

            var userId = Guid.Parse(principal.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var tokens = await _mediator.Send(new SilentRefreshCommand { UserId = userId, RefreshToken = refreshToken });

            if(tokens == null)
            {
                return Unauthorized();
            }

            return Ok(tokens);

        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _authOptions.Value.GetSymmetricSecurityKey(),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;

        }


    }
}
