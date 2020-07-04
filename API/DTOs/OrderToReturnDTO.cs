using System;
using System.Collections.Generic;
using Core.Entities.OrderAgregate;

namespace API.DTOs
{
    public class OrderToReturnDTO
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public Adress ShipToAdress { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal ShippingPrice { get; set; }
        public IReadOnlyList<OrderItemToReturnDTO> OrderItems { get; set; }
        public decimal Subtotal { get; set; }
        public string Status { get; set; }
        public decimal Total { get; set; }
    }
}