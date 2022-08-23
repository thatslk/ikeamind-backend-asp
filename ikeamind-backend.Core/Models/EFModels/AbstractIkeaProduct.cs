namespace ikeamind_backend.Core.Models.EFModels
{
    public abstract partial class AbstractIkeaProduct
    {
        public int Id { get; set; }
        public string Article { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Measure { get; set; }
        public float? Price { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string ArticlePrefix { get; set; }
        public string GlobalArticle { get; set; }
        public string GlobalArticlePrefix { get; set; }
    }
}
