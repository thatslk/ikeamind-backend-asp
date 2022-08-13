using ikeamind_backend.Core.CQRS.Commands.UserCommands.SetUserAvatar;
using ikeamind_backend.Core.CQRS.Commands.UserCommands.SetUserName;
using ikeamind_backend.Core.CQRS.Queries.UserQueries.AccountPage;
using ikeamind_backend.Core.CQRS.Queries.UserQueries.GetUserAvatar;
using ikeamind_backend.Core.Models.ReturnModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ikeamind_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetUserAvatar()
        {
            var userId = Guid.Parse(User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var result = await _mediator.Send(new GetUserAvatarQuery { UserId = userId });
            return Ok(new GetUserAvatarRM { Avatar = result });
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> AccountPage()
        {
            var userId = Guid.Parse(User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
            return Ok(await _mediator.Send(new AccountPageQuery { UserId = userId }));
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> SetUserAvatar(int avatarId)
        {
            var userId = Guid.Parse(User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var result = await _mediator.Send(new SetUserAvatarCommand { UserId = userId, NewAvatarId = avatarId });
            if(!result)
                return BadRequest("Указан несуществующий аватар");
            else 
                return Ok();
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> SetUserName(string username)
        {
            var userId = Guid.Parse(User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var result = await _mediator.Send(new SetUserNameCommand { UserId = userId, NewUserName = username });
            if (!result)
                return BadRequest("Пробелы в имени пользователя не поддерживаются");
            else
                return Ok();
        }

    }
}
