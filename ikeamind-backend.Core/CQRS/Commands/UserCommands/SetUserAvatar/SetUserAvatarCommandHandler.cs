using ikeamind_backend.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ikeamind_backend.Core.CQRS.Commands.UserCommands.SetUserAvatar
{
    public class SetUserAvatarCommand : IRequest<bool>
    {
        public Guid UserId { get; set; }
        public int NewAvatarId { get; set; }
    }

    public class SetUserAvatarCommandHandler
        : IRequestHandler<SetUserAvatarCommand, bool>
    {
        protected readonly IIkeaProductsAndUsersContext db;
        public SetUserAvatarCommandHandler(IIkeaProductsAndUsersContext context)
        {
            db = context;
        }

        public async Task<bool> Handle(SetUserAvatarCommand request, CancellationToken cancellationToken)
        {
            if(!db.Avatars.Select(x => x.Id).Contains(request.NewAvatarId))
            {
                return false;
            }

            var user = await(from u in db.Users
                             where u.Id == request.UserId
                             select u).SingleOrDefaultAsync();

            user.AvatarId = request.NewAvatarId;
            await db.SaveChangesAsync();
            return true;
        }
    }
}
