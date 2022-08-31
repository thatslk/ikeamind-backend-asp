using ikeamind_backend.Core.Interfaces;
using ikeamind_backend.Core.Models.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ikeamind_backend.Core.CQRS.Queries.UserQueries.GetUserAvatar
{
    public class GetUserAvatarQuery : IRequest<AvatarDTO>
    {
        public Guid UserId { get; set; }
    }

    public class GetUserAvatarQueryHandler
        : IRequestHandler<GetUserAvatarQuery, AvatarDTO>
    {
        protected readonly IIkeaProductsAndUsersContext db;
        public GetUserAvatarQueryHandler(IIkeaProductsAndUsersContext context)
        {
            db = context;
        }

        public async Task<AvatarDTO> Handle(GetUserAvatarQuery request, CancellationToken cancellationToken)
        {
            var user = await db.Users.SingleOrDefaultAsync(x => x.Id == request.UserId);
            var avatar = await db.Avatars.SingleOrDefaultAsync(x => x.Id == user.AvatarId);

            var avatarDescriptions = new Dictionary<string, string>();
            foreach(var avatarDescription in db.AvatarDescriptionByLocales.Where(x => x.AvatarId == avatar.Id))
            {
                avatarDescriptions.Add(avatarDescription.LocaleCode, avatarDescription.Description);
            }


            return new AvatarDTO
            {
                AvatarId = (int)avatar.Id,
                AvatarName = avatar.Title,
                AvatarDescriptionByLocales = avatarDescriptions,
                AvatarImageUrl = avatar.ImageUrl
            };
        }
    }
}
