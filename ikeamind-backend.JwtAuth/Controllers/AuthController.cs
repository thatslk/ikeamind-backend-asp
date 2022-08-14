using ikeamind_backend.Core.CQRS.Queries.UserQueries.AuthenticateUser;
using ikeamind_backend.Core.CQRS.Queries.UserQueries.CreateUser;
using ikeamind_backend.Core.CQRS.Queries.UserQueries.IsUsernameTaken;
using ikeamind_backend.Core.Models.EFModels.AccountModels;
using ikeamind_backend.Core.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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

            var token = GenerateJWT(account);
            return Ok(token);
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

            var token = GenerateJWT(createdAccount);
            return Ok(token);
        }

        private string GenerateJWT(Account user)
        {
            var authParams = _authOptions.Value;

            var securityKey = authParams.GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, user.Username),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            };

            var token = new JwtSecurityToken
                (authParams.Issuer,
                authParams.Audience,
                claims,
                expires: DateTime.Now.AddSeconds(authParams.TokenLifetimeInSeconds),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
