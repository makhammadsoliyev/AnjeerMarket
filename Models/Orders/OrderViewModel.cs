using AnjeerMarket.Models.Users;

namespace AnjeerMarket.Models.Orders;

public class OrderViewModel
{
    public long Id { get; set; }
    public UserViewModel User { get; set; }
    public DateTime Date { get; set; }
}