using AnjeerMarket.Models.Commons;

namespace AnjeerMarket.Models.CartItems;

public class CartItem : Auditable
{
    public long CartId { get; set; }
    public long ProductId { get; set; }
    public int Quantity { get; set; }
}