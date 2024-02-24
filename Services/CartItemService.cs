using AnjeerMarket.Configurations;
using AnjeerMarket.Extensions;
using AnjeerMarket.Helpers;
using AnjeerMarket.Interfaces;
using AnjeerMarket.Models.CartItems;

namespace AnjeerMarket.Services;

public class CartItemService : ICartItemService
{
    private List<CartItem> cartItems;
    private readonly ICartService cartService;
    private readonly IProductService productService;

    public CartItemService(ICartService cartService, IProductService productService)
    {
        this.cartService = cartService;
        this.productService = productService;
    }

    public async Task<CartItemViewModel> CreateAsync(CartItemCreationModel cartItem)
    {
        var cart = await cartService.GetById(cartItem.CartId);
        var product = await productService.GetByIdAsync(cartItem.ProductId);
        cartItems = await FileIO.ReadAsync<CartItem>(Constants.CART_ITEMS_PATH);

        var createdCartItem = cartItem.MapTo<CartItem>();
        createdCartItem.Id = cartItems.GenerateId();
        
        cartItems.Add(createdCartItem);

        await FileIO.WriteAsync(Constants.CART_ITEMS_PATH, cartItems);

        var res = createdCartItem.MapTo<CartItemViewModel>();
        res.Cart = cart;
        res.Product = product;

        return res;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        cartItems = await FileIO.ReadAsync<CartItem>(Constants.CART_ITEMS_PATH);
        var cartItem = cartItems.FirstOrDefault(ci => ci.Id == id && !ci.IsDeleted)
            ?? throw new Exception($"CartItem was not found with this id = {id}");

        cartItem.IsDeleted = true;
        cartItem.DeletedAt = DateTime.UtcNow;

        await FileIO.WriteAsync(Constants.CART_ITEMS_PATH, cartItems);

        return true;
    }

    public async Task<IEnumerable<CartItemViewModel>> GetAllAsync(long? cartId)
    {
        cartItems = await FileIO.ReadAsync<CartItem>(Constants.CART_ITEMS_PATH);
        cartItems = cartItems.FindAll(ci => !ci.IsDeleted);

        var result = new List<CartItemViewModel>();
        foreach (var cartItem in cartItems)
        {
            var cart = await cartService.GetById(cartItem.CartId);
            var product = await productService.GetByIdAsync(cartItem.ProductId);
            var ci = cartItem.MapTo<CartItemViewModel>();

            ci.Cart = cart;
            ci.Product = product;
            
            result.Add(ci);
        }

        return result;
    }

    public async Task<CartItemViewModel> GetById(long id)
    {
        cartItems = await FileIO.ReadAsync<CartItem>(Constants.CART_ITEMS_PATH);
        var cartItem = cartItems.FirstOrDefault(ci => ci.Id == id && !ci.IsDeleted)
            ?? throw new Exception($"CartItem was not found with this id = {id}");
        var cart = await cartService.GetById(cartItem.CartId);
        var product = await productService.GetByIdAsync(cartItem.ProductId);

        var res = cartItem.MapTo<CartItemViewModel>();
        res.Cart = cart;
        res.Product = product;

        return res;
    }
}
