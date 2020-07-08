namespace Core.Specifications
{
    public class OrderSpecParams : PaginationSpecParams
    {
        public int? Id { get; set; }
        public string Email { get; set; }
    }
}