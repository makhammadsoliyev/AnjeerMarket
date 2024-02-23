namespace AnjeerMarket.Models.OrderItems;

public class OrderItemCreationModel
{
    public long OrderId { get; set; }
    public long ProductId { get; set; }
    public int Quantity { get; set; }
}
