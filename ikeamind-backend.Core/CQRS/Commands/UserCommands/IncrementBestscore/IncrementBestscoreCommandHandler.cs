using ikeamind_backend.Core.Enums;
using ikeamind_backend.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ikeamind_backend.Core.CQRS.Commands.UserCommands.IncrementBestscore
{
    public class IncrementBestscoreCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; }
        public GameModesEnum GameMode { get; set; }
    }

    public class IncrementBestscoreCommandHandler
        : IRequestHandler<IncrementBestscoreCommand, Unit>
    {
        protected readonly IIkeaUsersContext db;
        public IncrementBestscoreCommandHandler(IIkeaUsersContext context)
        {
            db = context;
        }

        public async Task<Unit> Handle(IncrementBestscoreCommand request, CancellationToken cancellationToken)
        {
            var user = await db.Users.SingleAsync(x => x.Id == request.UserId.ToString());

            switch (request.GameMode)
            {
                case (GameModesEnum.TitleFirst):
                    user.BestscoreIf++;
                    break;
                case (GameModesEnum.PictureFirst):
                    user.BescorePf++;
                    break;
                default:
                    break;
            }

            await db.SaveChangesAsync();

            return new Unit();
        }
    }
}
