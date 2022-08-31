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
        protected readonly IIkeaProductsAndUsersContext db;
        public AccountPageQueryHandler(IIkeaProductsAndUsersContext context)
        {
            db = context;
        }

        public async Task<AccountPageRM> Handle(AccountPageQuery request, CancellationToken cancellationToken)
        {
            var user = await db.Users.SingleAsync(x => x.Id == request.UserId);

            var avatars = new List<AvatarDTO>();
            foreach(var avatar in db.Avatars.ToList())
            {
                var avatarDescriptions = new Dictionary<string, string>();
                foreach (var avatarDescription in db.AvatarDescriptionByLocales.Where(x => x.AvatarId == avatar.Id))
                {
                    avatarDescriptions.Add(avatarDescription.LocaleCode, avatarDescription.Description);
                }

                var toAdd = new AvatarDTO
                {
                    AvatarId = avatar.Id,
                    AvatarName = avatar.Title,
                    AvatarDescriptionByLocales = avatarDescriptions,
                    AvatarImageUrl = avatar.ImageUrl
                };

                avatars.Add(toAdd);
            }

            avatars = avatars.OrderBy(a => a.AvatarId).ToList();

            var toReturn = new AccountPageRM
            {
                UserId = user.Id,
                Username = user.Username,
                TitleFirstBestscore = (int)user.BestscoreTf,
                PictureFirstBestscore = (int)user.BestscorePf,
                Avatars = avatars,
                CurrentAvatarId = user.AvatarId,
            };

            return toReturn;
        }
    }
}
