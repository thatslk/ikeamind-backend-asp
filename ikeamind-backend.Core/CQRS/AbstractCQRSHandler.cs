using ikeamind_backend.Core.Interfaces;

namespace ikeamind_backend.Core.CQRS
{
    public abstract class AbstractCQRSHandler
    {
        protected readonly IIkeaProductsAndUsersContext db;
        public AbstractCQRSHandler(IIkeaProductsAndUsersContext context)
        {
            db = context;
        }
    }
}
