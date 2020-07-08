namespace Core.Specifications
{
    public class ProductSpecParams : PaginationSpecParams
    {
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string Sort { get; set; }
        private string _search;
        public string Search
        {
           get => _search;
           set => _search = value.ToLower();
        }
    }
}