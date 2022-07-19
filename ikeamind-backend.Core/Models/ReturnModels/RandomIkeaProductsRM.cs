using System.Collections.Generic;

namespace ikeamind_backend.Core.Models.ReturnModels
{
    public record RandomIkeaProductsRM
    {
        public List<IkeaProductRecord> Products { get; init; }
    }

    public record IkeaProductRecord
    {
        public string Article { get; init; }
        public string ProductName { get; init; }
        public string ProductCategory { get; init; }
        public string ImageURL { get; init; }
        public string ProductURL { get; init; }
    }
}
