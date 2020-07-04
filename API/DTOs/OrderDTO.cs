namespace API.DTOs
{
    public class OrderDTO
    {
        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public AdressDTO ShipToAdress { get; set; }
    }
}