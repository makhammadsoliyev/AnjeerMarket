namespace AnjeerMarket.Models.CartItems;

public class CartItemCreationModel
{
    public long CartId { get; set; }
    public long ProductId { get; set; }
    public int Quantity { get; set; }
}
