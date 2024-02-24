using AnjeerMarket.Configurations;
using AnjeerMarket.Extensions;
using AnjeerMarket.Helpers;
using AnjeerMarket.Interfaces;
using AnjeerMarket.Models.Carts;

namespace AnjeerMarket.Services;

public class CartService : ICartService
{
    private List<Cart> carts;
    private readonly IUserService userService;

    public CartService(IUserService userService)
    {
        this.userService = userService;
    }

    public async Task<CartViewModel> CreateAsync(CartCreationModel cart)
    {
        carts = await FileIO.ReadAsync<Cart>(Constants.CARTS_PATH);
        var user = await userService.GetByIdAsync(cart.UserId);

        var createdCart = cart.MapTo<Cart>();
        createdCart.Id = carts.GenerateId();

        carts.Add(createdCart);

        await FileIO.WriteAsync(Constants.CARTS_PATH, carts);

        var res = createdCart.MapTo<CartViewModel>();
        res.User = user;

        return res;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        carts = await FileIO.ReadAsync<Cart>(Constants.CARTS_PATH);
        var cart = carts.FirstOrDefault(c => c.Id == id && !c.IsDeleted)
            ?? throw new Exception($"Cart was not found with this id = {id}");

        cart.IsDeleted = true;
        cart.DeletedAt = DateTime.UtcNow;

        await FileIO.WriteAsync(Constants.CARTS_PATH, carts);

        return true;
    }

    public async Task<IEnumerable<CartViewModel>> GetAllAsync()
    {
        carts = await FileIO.ReadAsync<Cart>(Constants.CARTS_PATH);
        carts = carts.FindAll(c => !c.IsDeleted);

        var result = new List<CartViewModel>();
        foreach (var cart in carts)
        {
            var user = await userService.GetByIdAsync(cart.UserId);
            var c = cart.MapTo<CartViewModel>();
            c.User = user;

            result.Add(c);
        }

        return result;
    }

    public async Task<CartViewModel> GetById(long id)
    {
        carts = await FileIO.ReadAsync<Cart>(Constants.CARTS_PATH);
        var cart = carts.FirstOrDefault(c => c.Id == id && !c.IsDeleted)
            ?? throw new Exception($"Cart was not found with this id = {id}");

        var user = await userService.GetByIdAsync(cart.UserId);
        var res = cart.MapTo<CartViewModel>();
        res.User = user;

        return res;
    }
}
