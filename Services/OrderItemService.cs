using AnjeerMarket.Configurations;
using AnjeerMarket.Extensions;
using AnjeerMarket.Helpers;
using AnjeerMarket.Interfaces;
using AnjeerMarket.Models.OrderItems;

namespace AnjeerMarket.Services;

public class OrderItemService : IOrderItemService
{
    private List<OrderItem> orderItems;
    private readonly IOrderService orderService;
    private readonly IProductService productService;

    public OrderItemService(IProductService productService, IOrderService orderService)
    {
        this.orderService = orderService;
        this.productService = productService;
    }

    public async Task<OrderItemViewModel> CreateAsync(OrderItemCreationModel orderItem)
    {
        var order = await orderService.GetByIdAsync(orderItem.OrderId);
        var product = await productService.GetByIdAsync(orderItem.ProductId);
        orderItems = await FileIO.ReadAsync<OrderItem>(Constants.ORDER_ITEMS_PATH);

        var orderedItem = orderItem.MapTo<OrderItem>();
        orderedItem.Id = orderItems.GenerateId();

        orderItems.Add(orderedItem);

        await FileIO.WriteAsync(Constants.ORDER_ITEMS_PATH, orderItems);

        var res = orderedItem.MapTo<OrderItemViewModel>();
        res.Order = order;
        res.Product = product;

        return res;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        orderItems = await FileIO.ReadAsync<OrderItem>(Constants.ORDER_ITEMS_PATH);
        var orderItem = orderItems.FirstOrDefault(ot => ot.Id == id && !ot.IsDeleted)
            ?? throw new Exception($"OrderItem was not found with this id = {id}");

        orderItem.IsDeleted = true;
        orderItem.DeletedAt = DateTime.UtcNow;

        await FileIO.WriteAsync(Constants.ORDER_ITEMS_PATH, orderItems);
        
        return true;
    }

    public async Task<IEnumerable<OrderItemViewModel>> GetAllAsync(long? orderId = null)
    {
        orderItems = await FileIO.ReadAsync<OrderItem>(Constants.ORDER_ITEMS_PATH);
        orderItems = orderItems.FindAll(oi => !oi.IsDeleted);

        var result = new List<OrderItemViewModel>();
        foreach (var orderItem in orderItems)
        {
            var order = await orderService.GetByIdAsync(orderItem.OrderId);
            var product = await productService.GetByIdAsync(orderItem.ProductId);
            var item = orderItem.MapTo<OrderItemViewModel>();
            item.Order = order;
            item.Product = product;

            result.Add(item);
        }

        return result;
    }

    public async Task<OrderItemViewModel> GetByIdAsync(long id)
    {
        orderItems = await FileIO.ReadAsync<OrderItem>(Constants.ORDER_ITEMS_PATH);
        var orderItem = orderItems.FirstOrDefault(ot => ot.Id == id && !ot.IsDeleted)
            ?? throw new Exception($"OrderItem was not found with this id = {id}");
        var order = await orderService.GetByIdAsync(orderItem.OrderId);
        var product = await productService.GetByIdAsync(orderItem.ProductId);

        var item = orderItem.MapTo<OrderItemViewModel>();
        item.Order = order;
        item.Product = product;

        return item;
    }
}
