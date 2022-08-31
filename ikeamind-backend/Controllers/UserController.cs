using ikeamind_backend.Core.CQRS.Commands.UserCommands.IncrementBestscore;
using ikeamind_backend.Core.CQRS.Commands.UserCommands.SetUserAvatar;
using ikeamind_backend.Core.CQRS.Commands.UserCommands.SetUserName;
using ikeamind_backend.Core.CQRS.Queries.UserQueries.AccountPage;
using ikeamind_backend.Core.CQRS.Queries.UserQueries.GetBestscores;
using ikeamind_backend.Core.CQRS.Queries.UserQueries.GetUserAvatar;
using ikeamind_backend.Core.Enums;
using ikeamind_backend.Core.Models.ReturnModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static ikeamind_backend.Core.Constants.ErrorMessagesConstants;

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
                return BadRequest(ERR_US_INVALID_AVATARID);
            else 
                return Ok();
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> SetUserName(string username)
        {
            if(string.IsNullOrEmpty(username))
                return BadRequest(ERR_US_EMPTY_USERNAME);

            var userId = Guid.Parse(User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var result = await _mediator.Send(new SetUserNameCommand { UserId = userId, NewUserName = username });
            if (!result)
                return BadRequest(ERR_US_SPACES_IN_USERNAME);
            else
                return Ok();
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> IncrementBestscore(string mode)
        {
            var userId = Guid.Parse(User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var resultOfParsing = Enum.TryParse(mode, out GameModesEnum gameMode);
            if (resultOfParsing)
            {
                await _mediator.Send(new IncrementBestscoreCommand { UserId = userId, GameMode = gameMode });
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetBestscores()
        {
            var userId = Guid.Parse(User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
            return Ok(new GetBestscoresRM { Bestscores = await _mediator.Send(new GetBestscoresQuery { UserId = userId }) });
        }


    }
}
