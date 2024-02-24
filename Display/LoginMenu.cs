using AnjeerMarket.Enums;
using AnjeerMarket.Interfaces;
using AnjeerMarket.Models.Users;
using Spectre.Console;

namespace AnjeerMarket.Display;

public class LoginMenu
{
    private long id;
    private UserViewModel user;
    private readonly UserMenu userMenu;
    private readonly ProductMenu productMenu;
    private IEnumerable<UserViewModel> users;
    private readonly IUserService userService;
    private readonly CategoryMenu categoryMenu;
    private readonly SelectionMenu selectionMenu;

    public LoginMenu(IUserService userService, UserMenu userMenu, CategoryMenu categoryMenu, ProductMenu productMenu)
    {
        this.userMenu = userMenu;
        this.userService = userService;
        this.productMenu = productMenu;
        this.categoryMenu = categoryMenu;
        this.selectionMenu = new SelectionMenu();
    }

    public async Task LogIn()
    {
        string email = AnsiConsole.Ask<string>("[cyan1]Email: [/]");
        string password = AnsiConsole.Prompt<string>(new TextPrompt<string>("Enter your password:").Secret());

        try
        {
            id = await userService.LogInAsync(email, password);
            user = await userService.GetByIdAsync(id);
            await Display();
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
            AnsiConsole.MarkupLine("[blue]Enter to continue...[/]");
            Console.ReadKey();
        }
    }

    public async Task Display()
    {
        var circle = true;

        if (user.Role == Role.Admin)
            while (circle)
            {
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine($"[red]{user.FirstName} {user.LastName}, you are {user.Role}[/]");
                var selection = selectionMenu.ShowSelectionMenu("Choose one of options",
                    new string[] { "User", "Category", "Product", "Back" });

                switch (selection)
                {
                    case "User":
                        await userMenu.Display();
                        break;
                    case "Category":
                        await categoryMenu.Display();
                        break;
                    case "Product":
                        await productMenu.Display();
                        break;
                    case "Back":
                        circle = false;
                        break;
                }
            }
        else
        {

        }
    }
}