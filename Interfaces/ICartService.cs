using AnjeerMarket.Models.Carts;

namespace AnjeerMarket.Interfaces;

public interface ICartService
{
    Task<CartViewModel> CreateAsync(CartCreationModel cart);
    Task<CartViewModel> GetById(long id);
    Task<bool> DeleteAsync(long id);
    Task<IEnumerable<CartViewModel>> GetAllAsync();
}
