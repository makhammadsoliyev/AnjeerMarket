using AnjeerMarket.Models.Commons;

namespace AnjeerMarket.Models.Carts;

public class Cart : Auditable
{
    public long UserId { get; set; }
}