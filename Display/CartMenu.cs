using AnjeerMarket.Interfaces;
using AnjeerMarket.Models.CartItems;
using AnjeerMarket.Models.Carts;
using AnjeerMarket.Models.Products;
using AnjeerMarket.Models.Users;
using Spectre.Console;

namespace AnjeerMarket.Display;

public class CartMenu
{
    private readonly UserViewModel user;
    private readonly SelectionMenu selectionMenu;

    private readonly ICartService cartService;
    private readonly IProductService productService;
    private readonly ICartItemService cartItemService;

    private IEnumerable<CartViewModel> carts;
    private IEnumerable<CartItemViewModel> items;
    private IEnumerable<ProductViewModel> products;

    public CartMenu(UserViewModel user, ICartService cartService, ICartItemService cartItemService, IProductService productService)
    {
        this.user = user;
        this.cartService = cartService;
        this.productService = productService;
        this.cartItemService = cartItemService;
        this.selectionMenu = new SelectionMenu();
    }

    private async Task Add()
    {
        var userId = user.Id;

        var cartCreation = new CartCreationModel()
        {
            UserId = userId,
        };

        carts = await cartService.GetAllAsync();
        var cart = carts.FirstOrDefault(c => c.User.Id == userId)
            ?? await cartService.CreateAsync(cartCreation);
        products = await productService.GetAllAsync();
        var selection = string.Empty;

        AnsiConsole.MarkupLine("[blue]Choose products...[/]");

        while (true)
        {
            selection = selectionMenu.ShowSelectionMenu("Products", products.Select(p => $"{p.Id} {p.Name}").Reverse().Append("Finish").Reverse().ToArray());
            if (selection == "Finish")
                break;
            int quantity = AnsiConsole.Ask<int>("[yellow]Quantity: [/]");
            while (quantity <= 0)
            {
                AnsiConsole.MarkupLine($"[red]Invalid input.[/]");
                quantity = AnsiConsole.Ask<int>("[yellow]Quantity: [/]");
            }

            var productId = Convert.ToInt64(selection.Split()[0]);

            var cartItemCreation = new CartItemCreationModel()
            {
                CartId = cart.Id,
                ProductId = productId,
                Quantity = quantity,
            };
            var orderItem = await cartItemService.CreateAsync(cartItemCreation);
        }

        items = await cartItemService.GetAllAsync(cart.Id);
        var table = new SelectionMenu().DataTable("CartItems", items.ToArray());
        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("[blue]Enter to continue...[/]");
        Console.ReadKey();
    }

    private async Task GetById()
    {
        var userId = user.Id;

        var cartCreation = new CartCreationModel()
        {
            UserId = userId,
        };

        carts = await cartService.GetAllAsync();
        var cart = carts.FirstOrDefault(c => c.User.Id == userId)
            ?? await cartService.CreateAsync(cartCreation);

        items = await cartItemService.GetAllAsync(cart.Id);
        var table = new SelectionMenu().DataTable("OrdersItems", items.ToArray());
        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("[blue]Enter to continue...[/]");
        Console.ReadKey();
    }

    private async Task Delete()
    {
        var userId = user.Id;

        var cartCreation = new CartCreationModel()
        {
            UserId = userId,
        };

        carts = await cartService.GetAllAsync();
        var cart = carts.FirstOrDefault(c => c.User.Id == userId)
            ?? await cartService.CreateAsync(cartCreation);

        items = await cartItemService.GetAllAsync(cart.Id);

        var selection = selectionMenu.ShowSelectionMenu("Cart Items", items.Select(i => $"{i.Id} {i.Product.Name} {i.Quantity}").ToArray());
        var itemId = Convert.ToInt64(selection.Split()[0]);

        try
        {
            await cartItemService.DeleteAsync(itemId);
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
