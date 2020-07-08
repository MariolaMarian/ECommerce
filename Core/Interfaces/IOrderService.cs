using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.OrderAgregate;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Adress shippingAdress);

        Task<IReadOnlyList<Order>> GetOrdersForUserAsync(OrderSpecParams orderSpecParams);
        Task<int> GetCountOrdersForUserAsync(OrderSpecParams orderSpecParams);

        Task<Order> GetOrderByIdAsync(int id, string buyerEmail);

        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
    }
}