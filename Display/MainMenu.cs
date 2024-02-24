using AnjeerMarket.Interfaces;
using AnjeerMarket.Services;
using Spectre.Console;

namespace AnjeerMarket.Display;

public class MainMenu
{
    private readonly IUserService userService;
    private readonly ICartService cartService;
    private readonly IOrderService orderService;
    private readonly IProductService productService;
    private readonly ICartItemService cartItemService;
    private readonly ICategoryService categoryService;
    private readonly IOrderItemService orderItemService;

    private readonly UserMenu userMenu;
    private readonly LoginMenu loginMenu;
    private readonly ProductMenu productMenu;
    private readonly CategoryMenu categoryMenu;
    private readonly RegistrationMenu registrationMenu;

    public MainMenu()
    {
        this.userService = new UserService();
        this.categoryService = new CategoryService();
        this.cartService = new CartService(userService);
        this.orderService = new OrderService(userService);
        this.productService = new ProductService(categoryService);
        this.cartItemService = new CartItemService(cartService, productService);
        this.orderItemService = new OrderItemService(productService, orderService);

        this.userMenu = new UserMenu(userService);
        this.categoryMenu = new CategoryMenu(categoryService);
        this.registrationMenu = new RegistrationMenu(userService);
        this.productMenu = new ProductMenu(productService, categoryService);
        this.loginMenu = new LoginMenu(userService, userMenu, categoryMenu, productMenu, orderService, productService, orderItemService, cartService, cartItemService);
    }

    public async Task Main()
    {
        var circle = true;
        var selectionDisplay = new SelectionMenu();

        while (circle)
        {
            AnsiConsole.Clear();
            var selection = selectionDisplay.ShowSelectionMenu(
                "Choose one of options",
                new string[] { "Registration", "Login", "Exit" });

            switch (selection)
            {
                case "Registration":
                    await registrationMenu.Add();
                    break;
                case "Login":
                    await loginMenu.LogIn();
                    break;
                case "Exit":
                    circle = false;
                    break;
            }
        }
    }
}
