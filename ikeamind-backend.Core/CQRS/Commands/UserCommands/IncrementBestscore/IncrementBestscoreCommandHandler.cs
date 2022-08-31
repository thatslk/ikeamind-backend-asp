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
        protected readonly IIkeaProductsAndUsersContext db;
        public IncrementBestscoreCommandHandler(IIkeaProductsAndUsersContext context)
        {
            db = context;
        }

        public async Task<Unit> Handle(IncrementBestscoreCommand request, CancellationToken cancellationToken)
        {
            var user = await db.Users.SingleAsync(x => x.Id == request.UserId);

            switch (request.GameMode)
            {
                case (GameModesEnum.TitleFirst):
                    user.BestscoreTf++;
                    break;
                case (GameModesEnum.PictureFirst):
                    user.BestscorePf++;
                    break;
                default:
                    break;
            }

            await db.SaveChangesAsync();

            return new Unit();
        }
    }
}
