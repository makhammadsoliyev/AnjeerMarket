using AnjeerMarket.Models.Users;

namespace AnjeerMarket.Models.Carts;

public class CartViewModel
{
    public long Id { get; set; }
    public UserViewModel User { get; set; }
}