using AnjeerMarket.Configurations;
using AnjeerMarket.Extensions;
using AnjeerMarket.Helpers;
using AnjeerMarket.Interfaces;
using AnjeerMarket.Models.Orders;

namespace AnjeerMarket.Services;

public class OrderService : IOrderService
{
    private List<Order> orders;
    private readonly IUserService userService;

    public OrderService(IUserService userService)
    {
        this.userService = userService;
    }

    public async Task<OrderViewModel> CreateAsync(OrderCreationModel order)
    {
        var user = await userService.GetByIdAsync(order.UserId);
        orders = await FileIO.ReadAsync<Order>(Constants.ORDERS_PATH);

        var createdOrder = order.MapTo<Order>();
        createdOrder.Id = orders.GenerateId();

        orders.Add(createdOrder);

        await FileIO.WriteAsync(Constants.ORDERS_PATH, orders);

        var res = createdOrder.MapTo<OrderViewModel>();
        res.User = user;

        return res;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        orders = await FileIO.ReadAsync<Order>(Constants.ORDERS_PATH);
        var order = orders.FirstOrDefault(o => o.Id == id && !o.IsDeleted)
            ?? throw new Exception($"Order was not found with this id");

        order.IsDeleted = true;
        order.DeletedAt = DateTime.UtcNow;

        await FileIO.WriteAsync(Constants.ORDERS_PATH, orders);

        return true;
    }

    public async Task<IEnumerable<OrderViewModel>> GetAllAsync(long? userId = null)
    {
        orders = await FileIO.ReadAsync<Order>(Constants.ORDERS_PATH);
        orders = orders.FindAll(o => !o.IsDeleted && o.UserId == userId);

        var result = new List<OrderViewModel>();
        foreach (var order in orders)
        {
            var user = await userService.GetByIdAsync(order.UserId);
            var o = order.MapTo<OrderViewModel>();
            o.User = user;

            result.Add(o);
        }

        return result;
    }

    public async Task<OrderViewModel> GetByIdAsync(long id)
    {
        orders = await FileIO.ReadAsync<Order>(Constants.ORDERS_PATH);
        var order = orders.FirstOrDefault(o => o.Id == id && !o.IsDeleted)
            ?? throw new Exception($"Order was not found with this id = {id}");

        var user = await userService.GetByIdAsync(order.UserId);
        var res = order.MapTo<OrderViewModel>();
        res.User = user;

        return res;
    }
}
