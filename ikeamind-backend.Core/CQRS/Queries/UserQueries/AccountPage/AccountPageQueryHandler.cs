using ikeamind_backend.Core.Interfaces;
using ikeamind_backend.Core.Models.DTOs;
using ikeamind_backend.Core.Models.ReturnModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ikeamind_backend.Core.CQRS.Queries.UserQueries.AccountPage
{
    public class AccountPageQuery : IRequest<AccountPageRM>
    {
        public Guid UserId { get; set; }
    }

    public class AccountPageQueryHandler
        : IRequestHandler<AccountPageQuery, AccountPageRM>
    {
        protected readonly IIkeaUsersContext db;
        public AccountPageQueryHandler(IIkeaUsersContext context)
        {
            db = context;
        }

        public async Task<AccountPageRM> Handle(AccountPageQuery request, CancellationToken cancellationToken)
        {
            var user = await db.Users.SingleAsync(x => x.Id == request.UserId.ToString());

            var avatars = new List<AvatarDTO>();
            foreach(var avatar in db.Avatars)
            {
                var toAdd = new AvatarDTO
                {
                    AvatarId = (int)avatar.Id,
                    AvatarName = avatar.Title,
                    AvatarDescriptionByLocales = new Dictionary<string, string>
                    {
                        { "ru", avatar.Description }
                    },
                    AvatarImageUrl = avatar.ImageUrl
                };

                avatars.Add(toAdd);
            }

            var toReturn = new AccountPageRM
            {
                UserId = Guid.Parse(user.Id),
                Username = user.Username,
                TitleFirstBestscore = (int)user.BestscoreIf,
                PictureFirstBestscore = (int)user.BescorePf,
                Avatars = avatars,
                CurrentAvatarId = (int)user.AvatarId,
            };

            return toReturn;
        }
    }
}
