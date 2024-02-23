using AnjeerMarket.Models.Orders;
using AnjeerMarket.Models.Products;

namespace AnjeerMarket.Models.OrderItems;

public class OrderItemViewModel
{
    public long Id { get; set; }
    public OrderViewModel Order { get; set; }
    public ProductViewModel Product { get; set; }
    public int Quantity { get; set; }
}