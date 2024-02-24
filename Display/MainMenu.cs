using AnjeerMarket.Interfaces;
using AnjeerMarket.Services;
using Spectre.Console;

namespace AnjeerMarket.Display;

public class MainMenu
{
    private readonly RegistrationMenu registrationMenu;
    private readonly LoginMenu loginMenu;
    private readonly IUserService userService;
    private readonly ProductMenu productMenu;
    private readonly UserMenu userMenu;
    private readonly CategoryMenu categoryMenu;
    private readonly ICategoryService categoryService;
    private readonly IProductService productService;

    public MainMenu()
    {
        userService = new UserService();
        categoryService = new CategoryService();
        productService = new ProductService(categoryService);
        userMenu = new UserMenu(userService);
        categoryMenu = new CategoryMenu(categoryService);
        productMenu = new ProductMenu(productService, categoryService);
        registrationMenu = new RegistrationMenu(userService);
        loginMenu = new LoginMenu(userService, userMenu, categoryMenu, productMenu);
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
