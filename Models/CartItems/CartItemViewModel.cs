using AnjeerMarket.Models.Carts;
using AnjeerMarket.Models.Products;

namespace AnjeerMarket.Models.CartItems;

public class CartItemViewModel
{
    public long Id { get; set; }
    public CartViewModel Cart { get; set; }
    public ProductViewModel Product { get; set; }
    public int Quantity { get; set; }
}