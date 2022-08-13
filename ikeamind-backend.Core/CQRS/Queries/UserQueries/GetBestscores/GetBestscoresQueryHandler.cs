using ikeamind_backend.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ikeamind_backend.Core.CQRS.Queries.UserQueries.GetBestscores
{
    public class GetBestscoresQuery : IRequest<Dictionary<string, int>>
    {
        public Guid UserId { get; set; }
    }

    public class GetBestscoresQueryHandler
        : IRequestHandler<GetBestscoresQuery, Dictionary<string, int>>
    {
        protected readonly IIkeaUsersContext db;
        public GetBestscoresQueryHandler(IIkeaUsersContext context)
        {
            db = context;
        }

        public async Task<Dictionary<string, int>> Handle(GetBestscoresQuery request, CancellationToken cancellationToken)
        {
            var user = await db.Users.SingleOrDefaultAsync(x => x.Id == request.UserId.ToString());
            return new Dictionary<string, int>()
            {
                { "tf_bestscore", (int)user.BestscoreIf },
                { "pf_bestscore", (int)user.BescorePf },
            };
        }
    }
}
