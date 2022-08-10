using ikeamind_backend.Core.Enums;
using ikeamind_backend.Core.Interfaces;
using ikeamind_backend.Core.Models.EFModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ikeamind_backend.Core.CQRS.Queries.GetNextRandomProducts
{
    public class GetNextRandomProductsQuery : IRequest<List<AbstractIkeaProduct>>
    {
        public int Amount { get; set; }
        public DBLocalesEnum Locale{ get; set; }
    }

    public class GetNextRandomProductsQueryHandler
        : AbstractCQRSHandler, IRequestHandler<GetNextRandomProductsQuery, List<AbstractIkeaProduct>>
    {
        public GetNextRandomProductsQueryHandler
            (IIkeaDbContext context) : base(context) { }

        public async Task<List<AbstractIkeaProduct>> Handle(GetNextRandomProductsQuery request, CancellationToken cancellationToken)
        {
            Random random = new Random();
            List<AbstractIkeaProduct> products = new List<AbstractIkeaProduct>();

            IQueryable<AbstractIkeaProduct> selectedTable;    

            switch (request.Locale)
            {
                case (DBLocalesEnum.us):
                    selectedTable = db.IkeaUs;
                    break;
                case (DBLocalesEnum.se):
                    selectedTable = db.IkeaSe;
                    break;
                case (DBLocalesEnum.ru):
                    selectedTable = db.IkeaRu;
                    break;
                default:
                    selectedTable = db.IkeaUs;
                    break;
            }


            var foundProductTitles = new HashSet<string>();
            
            while(products.Count < request.Amount)
            {
                var randomPosition = random.Next(selectedTable.Count());
                var randomProduct = await selectedTable.Skip(randomPosition).FirstAsync();
                if(!foundProductTitles.Contains(randomProduct.Name))
                {
                    foundProductTitles.Add(randomProduct.Name);
                    products.Add(randomProduct);
                }
            }

            return products;
        }
    }
}
