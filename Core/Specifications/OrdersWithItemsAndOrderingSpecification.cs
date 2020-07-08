using System;
using System.Linq.Expressions;
using Core.Entities.OrderAgregate;

namespace Core.Specifications
{
    public class OrdersWithItemsAndDeliveryMethodSpecification : BaseSpecification<Order>
    {
        public OrdersWithItemsAndDeliveryMethodSpecification(OrderSpecParams orderParams) : base(o=> o.BuyerEmail == orderParams.Email)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
            AddOrderByDescending(o => o.OrderDate);
            ApplyPaging(orderParams.PageSize * (orderParams.PageIndex - 1), orderParams.PageSize);
        }

        public OrdersWithItemsAndDeliveryMethodSpecification(int id, string email) : base(o => o.Id == id && o.BuyerEmail == email)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
        }
    }
}