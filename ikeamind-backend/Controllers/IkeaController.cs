using ikeamind_backend.Core.CQRS.Queries.GetNextRandomProducts;
using ikeamind_backend.Core.Enums;
using ikeamind_backend.Core.Models.ReturnModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ikeamind_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IkeaController : Controller
    {
        private readonly IMediator _mediator;
        public IkeaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetNextThreeProducts([FromQuery]string locale)
        {
            Enum.TryParse(locale, out DBLocalesEnum localeEnum);

            var randomProducts = await _mediator.Send(new GetNextRandomProductsQuery
            {
                Amount = 3,
                Locale = localeEnum
            });

            List<IkeaProductRecord> retProuducts = new();
            foreach(var p in randomProducts)
            {
                retProuducts.Add(new IkeaProductRecord 
                { 
                    Article = p.Article,
                    ImageURL = p.ImageUrl + "?f=xxxs",
                    ProductCategory = p.Category,
                    ProductName = p.Name,
                    ProductURL = p.Url
                });
            }


            var rm = new RandomIkeaProductsRM { Products = retProuducts };
            return Ok(rm);
        }
    }
}
