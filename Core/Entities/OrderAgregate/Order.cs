using System;
using System.Collections.Generic;

namespace Core.Entities.OrderAgregate
{
    public class Order : BaseEntity
    {
        public Order()
        {
        }

        public Order(IReadOnlyList<OrderItem> orderItems, string buyerEmail,  Adress shipToAdress, DeliveryMethod deliveryMethod,  decimal subtotal)
        {
            BuyerEmail = buyerEmail;
            ShipToAdress = shipToAdress;
            DeliveryMethod = deliveryMethod;
            OrderItems = orderItems;
            Subtotal = subtotal;
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public Adress ShipToAdress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public IReadOnlyList<OrderItem> OrderItems { get; set; }
        public decimal Subtotal { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public string PaymentIntentId { get; set; }
        public decimal GetTotal()
        {
            return Subtotal + DeliveryMethod.Price;
        }

    }
}