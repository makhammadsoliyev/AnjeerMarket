using AnjeerMarket.Configurations;
using AnjeerMarket.Enums;
using AnjeerMarket.Extensions;
using AnjeerMarket.Helpers;
using AnjeerMarket.Interfaces;
using AnjeerMarket.Models.Users;

namespace AnjeerMarket.Services;

public class UserService : IUserService
{
    private List<User> users;

    public async Task<UserViewModel> CreateAsync(UserCreationModel user)
    {
        users = await FileIO.ReadAsync<User>(Constants.USERS_PATH);
        var existUser = users.FirstOrDefault(u => u.Email.Equals(user.Email));
        if (existUser is not null && existUser.IsDeleted)
            return await UpdateAsync(existUser.Id, user.MapTo<UserUpdateModel>(), true);

        if (existUser is not null)
            throw new Exception($"User already exists with this email = {user.Email}");

        var createdUser = user.MapTo<User>();
        createdUser.Id = users.GenerateId();
        users.Add(createdUser);

        await FileIO.WriteAsync(Constants.USERS_PATH, users);

        return createdUser.MapTo<UserViewModel>();
    }

    public async Task<bool> DeleteAsync(long id)
    {
        users = await FileIO.ReadAsync<User>(Constants.USERS_PATH);
        var user = users.FirstOrDefault(u => u.Id == id && !u.IsDeleted)
            ?? throw new Exception($"User was not found with this id = {id}");

        user.IsDeleted = true;
        user.DeletedAt = DateTime.UtcNow;

        await FileIO.WriteAsync(Constants.USERS_PATH, users);

        return true;
    }

    public async Task<IEnumerable<UserViewModel>> GetAllAsync(Role? role = null)
    {
        users = await FileIO.ReadAsync<User>(Constants.USERS_PATH);
        if (role is null)
            users = users.FindAll(u => !u.IsDeleted);
        else
            users = users.FindAll(u => !u.IsDeleted && u.Role == role);

        return users.Select(u => u.MapTo<UserViewModel>());
    }

    public async Task<UserViewModel> GetByIdAsync(long id)
    {
        users = await FileIO.ReadAsync<User>(Constants.USERS_PATH);
        var user = users.FirstOrDefault(u => u.Id == id && !u.IsDeleted)
            ?? throw new Exception($"User was not found with this id = {id}");

        return user.MapTo<UserViewModel>();
    }

    public async Task<UserViewModel> UpdateAsync(long id, UserUpdateModel user, bool isUsedDeleted = false)
    {
        users = (await FileIO.ReadAsync<User>(Constants.USERS_PATH)).ToList();
        var existUser = new User();
        if (isUsedDeleted)
            existUser = users.FirstOrDefault(u => u.Id == id);
        else
            existUser = users.FirstOrDefault(u => u.Id == id && !u.IsDeleted)
            ?? throw new Exception($"User was not found with this id = {id}");

        existUser.Role = user.Role;
        existUser.IsDeleted = false;
        existUser.Email = user.Email;
        existUser.Password = user.Password;
        existUser.LastName = user.LastName;
        existUser.FirstName = user.FirstName;
        existUser.UpdatedAt = DateTime.UtcNow;

        await FileIO.WriteAsync(Constants.USERS_PATH, users);

        return existUser.MapTo<UserViewModel>();
    }
}
