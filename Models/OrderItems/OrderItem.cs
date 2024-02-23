using AnjeerMarket.Models.Commons;

namespace AnjeerMarket.Models.OrderItems;

public class OrderItem : Auditable
{
    public long OrderId { get; set; }
    public long ProductId { get; set; }
    public int Quantity { get; set; }
}