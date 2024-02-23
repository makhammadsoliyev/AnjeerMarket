using AnjeerMarket.Models.Orders;

namespace AnjeerMarket.Interfaces;

public interface IOrderService
{
    Task<OrderViewModel> CreateAsync(OrderCreationModel order);
    Task<OrderViewModel> GetByIdAsync(long id);
    Task<bool> DeleteAsync(long id);
    Task<IEnumerable<OrderViewModel>> GetAllAsync(long? userId = null);
}