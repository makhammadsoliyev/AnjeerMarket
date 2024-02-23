using AnjeerMarket.Models.CartItems;

namespace AnjeerMarket.Interfaces;

public interface ICartItemService
{
    Task<CartItemViewModel> CreateAsync(CartItemCreationModel cartItem);
    Task<CartItemViewModel> GetById(long id);
    Task<bool> DeleteAsync(long id);
    Task<IEnumerable<CartItemViewModel>> GetAllAsync(long? cartId);
}
