using ikeamind_backend.Core.CQRS.Commands.UserCommands.SetUserTokens;
using ikeamind_backend.Core.CQRS.Commands.UserCommands.SilentRefresh;
using ikeamind_backend.Core.CQRS.Queries.UserQueries.AuthenticateUser;
using ikeamind_backend.Core.CQRS.Queries.UserQueries.CreateUser;
using ikeamind_backend.Core.CQRS.Queries.UserQueries.IsUsernameTaken;
using ikeamind_backend.Core.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
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
        private readonly ExtractPrincipalFromExpiredToken _expiredTokenExtractor;
        public AuthController
            (IMediator mediator, 
            IOptions<AuthOptions> authOptions,
            ExtractPrincipalFromExpiredToken expiredTokenExtractor)
        {
            _mediator = mediator;
            _authOptions = authOptions;
            _expiredTokenExtractor = expiredTokenExtractor;
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

            var principal = _expiredTokenExtractor.GetPrincipalFromExpiredToken(accessToken);
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

    }
}
