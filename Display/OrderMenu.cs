using AnjeerMarket.Interfaces;
using AnjeerMarket.Models.OrderItems;
using AnjeerMarket.Models.Orders;
using AnjeerMarket.Models.Products;
using AnjeerMarket.Models.Users;
using Spectre.Console;

namespace AnjeerMarket.Display;

public class OrderMenu
{
    private readonly UserViewModel user;
    private readonly SelectionMenu selectionMenu;

    private readonly IOrderService orderService;
    private readonly IProductService productService;
    private readonly IOrderItemService orderItemService;

    private IEnumerable<OrderViewModel> orders;
    private IEnumerable<OrderItemViewModel> items;
    private IEnumerable<ProductViewModel> products;

    public OrderMenu(UserViewModel user, IOrderService orderService, IOrderItemService orderItemService, IProductService productService)
    {
        this.user = user;
        this.orderService = orderService;
        this.productService = productService;
        this.orderItemService = orderItemService;
        this.selectionMenu = new SelectionMenu();
    }

    private async Task Add()
    {
        var userId = user.Id;
        var orderCreation = new OrderCreationModel()
        {
            UserId = userId,
        };

        var order = await orderService.CreateAsync(orderCreation);
        products = await productService.GetAllAsync();
        var selection = string.Empty;
        AnsiConsole.MarkupLine("[blue]Choose products...[/]");

        while (true)
        {
            selection = selectionMenu.ShowSelectionMenu("Products", products.Select(p => $"{p.Id} {p.Name}").Append("Finish").ToArray());
            if (selection == "Finish")
                break;
            int quantity = AnsiConsole.Ask<int>("[yellow]Quantity: [/]");
            while (quantity <= 0)
            {
                AnsiConsole.MarkupLine($"[red]Invalid input.[/]");
                quantity = AnsiConsole.Ask<int>("[yellow]Quantity: [/]");
            }

            var productId = Convert.ToInt64(selection.Split()[0]);

            var orderItemCreation = new OrderItemCreationModel()
            {
                OrderId = order.Id,
                ProductId = productId,
                Quantity = quantity,
            };
            var orderItem = await orderItemService.CreateAsync(orderItemCreation);
        }

        items = await orderItemService.GetAllAsync(order.Id);
        var table = new SelectionMenu().DataTable("Orders", items.ToArray());
        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("[blue]Enter to continue...[/]");
        Console.ReadKey();
    }

    private async Task GetById()
    {
        orders = await orderService.GetAllAsync(user.Id);
        var selection = selectionMenu.ShowSelectionMenu("Orders", orders.Select(o => $"{o.Id} {o.User.FirstName} {o.User.LastName} {o.Date}").ToArray());
        var orderId = Convert.ToInt64(selection.Split()[0]);
        items = await orderItemService.GetAllAsync(orderId);
        var table = new SelectionMenu().DataTable("OrdersItems", items.ToArray());
        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("[blue]Enter to continue...[/]");
        Console.ReadKey();
    }

    private async Task Delete()
    {
        orders = await orderService.GetAllAsync(user.Id);
        var selection = selectionMenu.ShowSelectionMenu("Orders", orders.Select(o => $"{o.Id} {o.User.FirstName} {o.User.LastName} {o.Date}").ToArray());
        var orderId = Convert.ToInt64(selection.Split()[0]);
        try
        {
            items = await orderItemService.GetAllAsync(orderId);
            bool isDeleted = await orderService.DeleteAsync(orderId);
            foreach (var item in items)
                await orderItemService.DeleteAsync(item.Id);
            AnsiConsole.MarkupLine("[green]Successfully deleted...[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        }
        Thread.Sleep(1500);
    }

    public async Task Display()
    {
        var circle = true;

        while (circle)
        {
            AnsiConsole.Clear();
            var selection = selectionMenu.ShowSelectionMenu("Choose one of options",
                    new string[] { "Add", "GetById", "Delete", "Back" });

            switch (selection)
            {
                case "Add":
                    await Add();
                    break;
                case "GetById":
                    await GetById();
                    break;
                case "Delete":
                    await Delete();
                    break;
                case "Back":
                    circle = false;
                    break;
            }
        }
    }
}
