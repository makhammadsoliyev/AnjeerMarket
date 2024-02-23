using AnjeerMarket.Enums;
using AnjeerMarket.Models.Users;

namespace AnjeerMarket.Interfaces;

public interface IUserService
{
    Task<UserViewModel> CreateAsync(UserCreationModel user);
    Task<UserViewModel> GetByIdAsync(long id);
    Task<UserViewModel> UpdateAsync(long id, UserUpdateModel user);
    Task<bool> DeleteAsync(long id);
    Task<IEnumerable<UserViewModel>> GetAllAsync(Role? role = null);
}
