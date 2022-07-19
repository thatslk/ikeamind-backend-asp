using ikeamind_backend.Core.Interfaces;

namespace ikeamind_backend.Core.CQRS
{
    public abstract class AbstractCQRSHandler
    {
        protected readonly IIkeaDbContext db;
        public AbstractCQRSHandler(IIkeaDbContext context)
        {
            db = context;
        }
    }
}
