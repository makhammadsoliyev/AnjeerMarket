using AnjeerMarket.Enums;
using AnjeerMarket.Models.Commons;

namespace AnjeerMarket.Models.Users;

public class User : Auditable
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
}