using ikeamind_backend.Core.CQRS.Commands.UserCommands.SetUserTokens;
using ikeamind_backend.Core.CQRS.Commands.UserCommands.SilentRefresh;
using ikeamind_backend.Core.CQRS.Queries.UserQueries.AuthenticateUser;
using ikeamind_backend.Core.CQRS.Queries.UserQueries.CreateUser;
using ikeamind_backend.Core.CQRS.Queries.UserQueries.IsUsernameTaken;
using ikeamind_backend.Core.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static ikeamind_backend.Core.Constants.ErrorMessagesConstants;


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
            if (string.IsNullOrEmpty(username))
                return BadRequest(ERR_US_EMPTY_USERNAME);

            if (username.Contains(' '))
            {
                return BadRequest(ERR_US_SPACES_IN_USERNAME);
            }

            var account = await _mediator.Send(new AuthenticateUserQuery { Username = username, Password = password });
            if (account == null)
            {
                return BadRequest(ERR_US_USER_NOTFOUND);
            }

            var tokens = await _mediator.Send(new SetUserTokensCommand { UserId = account.Id });
            return Ok(tokens);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Signup(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
                return BadRequest(ERR_US_EMPTY_USERNAME);

            if (username.Contains(' '))
            {
                return BadRequest(ERR_US_SPACES_IN_USERNAME);
            }

            if (await _mediator.Send(new IsUsernameTakenQuery { Username = username }))
            {
                return BadRequest(ERR_US_USER_ALREADY_EXISTS);
            }

            var createdAccount = await _mediator.Send(new CreateUserQuery { Username = username, Password = password });

            var tokens = await _mediator.Send(new SetUserTokensCommand { UserId = createdAccount.Id });
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
