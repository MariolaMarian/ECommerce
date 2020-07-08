using Core.Entities.OrderAgregate;

namespace Core.Specifications
{
    public class OrdersWithItemsAndDeliveryMethodCountSpecification : BaseSpecification<Order>
    {
        public OrdersWithItemsAndDeliveryMethodCountSpecification(OrderSpecParams orderParams) : base(o=> o.BuyerEmail == orderParams.Email)
        {
        }
    }
}