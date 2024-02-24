using AnjeerMarket.Enums;
using AnjeerMarket.Models.Users;

namespace AnjeerMarket.Interfaces;

public interface IUserService
{
    Task<long> LogInAsync(string email, string password);
    Task<UserViewModel> CreateAsync(UserCreationModel user);
    Task<UserViewModel> GetByIdAsync(long id);
    Task<UserViewModel> UpdateAsync(long id, UserUpdateModel user, bool isUsedDeleted = false);
    Task<bool> DeleteAsync(long id);
    Task<IEnumerable<UserViewModel>> GetAllAsync(Role? role = null);
}
