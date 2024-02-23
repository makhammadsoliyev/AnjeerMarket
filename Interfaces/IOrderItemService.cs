using AnjeerMarket.Models.OrderItems;

namespace AnjeerMarket.Interfaces;

public interface IOrderItemService
{
    Task<OrderItemViewModel> CreateAsync(OrderItemCreationModel orderItem);
    Task<OrderItemViewModel> GetByIdAsync(long id);
    Task<bool> DeleteAsync(long id);
    Task<IEnumerable<OrderItemViewModel>> GetAllAsync(long? orderId = null);
}