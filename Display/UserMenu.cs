using AnjeerMarket.Enums;
using AnjeerMarket.Interfaces;
using AnjeerMarket.Models.Users;
using Spectre.Console;
using System.Text.RegularExpressions;

namespace AnjeerMarket.Display;

public class UserMenu
{
    private IEnumerable<UserViewModel> users;
    private readonly IUserService userService;
    private readonly SelectionMenu selectionMenu;

    public UserMenu(IUserService userService)
    {
        this.userService = userService;
        this.selectionMenu = new SelectionMenu();
    }

    private async Task GetById()
    {
        users = await userService.GetAllAsync();
        var selection = selectionMenu.ShowSelectionMenu("Users", users.Select(u => $"{u.Id} {u.FirstName} {u.LastName}").ToArray());
        var id = Convert.ToInt64(selection.Split()[0]);
        try
        {
            var user = await userService.GetByIdAsync(id);
            var table = new SelectionMenu().DataTable("User", user);
            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine("[blue]Enter to continue...[/]");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
            Thread.Sleep(1500);
        }
    }

    private async Task Update()
    {
        users = await userService.GetAllAsync();
        var selection = selectionMenu.ShowSelectionMenu("Users", users.Select(u => $"{u.Id} {u.FirstName} {u.LastName}").ToArray());
        var id = Convert.ToInt64(selection.Split()[0]);

        string firstName = AnsiConsole.Ask<string>("[blue]FirstName: [/]");
        string lastName = AnsiConsole.Ask<string>("[cyan2]LastName: [/]");
        string email = AnsiConsole.Ask<string>("[cyan1]Email: [/]");
        while (!Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
        {
            AnsiConsole.MarkupLine("[red]Invalid input.[/]");
            email = AnsiConsole.Ask<string>("[cyan1]Email: [/]");
        }
        string password = AnsiConsole.Prompt<string>(new TextPrompt<string>("Enter your password:").Secret());
        while (!Regex.IsMatch(password, @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$"))
        {
            AnsiConsole.MarkupLine("[red]Invalid input.[/]");
            password = AnsiConsole.Prompt<string>(new TextPrompt<string>("Enter your password:").Secret());
        }
        string role = new SelectionMenu().ShowSelectionMenu("Choose user role", Enum.GetNames<Role>());

        var user = new UserUpdateModel()
        {
            Email = email,
            Password = password,
            LastName = lastName,
            FirstName = firstName,
            Role = Enum.Parse<Role>(role),
        };

        try
        {
            var updatedUser = await userService.UpdateAsync(id, user);
            AnsiConsole.MarkupLine("[green]Successfully updated...[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        }
        Thread.Sleep(1500);
    }

    private async Task Delete()
    {
        users = await userService.GetAllAsync();
        var selection = selectionMenu.ShowSelectionMenu("Users", users.Select(u => $"{u.Id} {u.FirstName} {u.LastName}").ToArray());
        var id = Convert.ToInt64(selection.Split()[0]);

        try
        {
            bool isDeleted = await userService.DeleteAsync(id);
            AnsiConsole.MarkupLine("[green]Successfully deleted...[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        }
        Thread.Sleep(1500);
    }

    private async Task GetAll()
    {
        var users = await userService.GetAllAsync();
        var table = new SelectionMenu().DataTable("Users", users.ToArray());
        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("[blue]Enter to continue...[/]");
        Console.ReadKey();
    }

    private async Task GetAllByUserRole()
    {
        string role = new SelectionMenu().ShowSelectionMenu("Choose user role", Enum.GetNames<Role>());
        var users = await userService.GetAllAsync(Enum.Parse<Role>(role));
        var table = new SelectionMenu().DataTable("Users", users.ToArray());
        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("[blue]Enter to continue...[/]");
        Console.ReadKey();
    }

    public async Task Display()
    {
        var circle = true;

        while (circle)
        {
            AnsiConsole.Clear();
            var selection = selectionMenu.ShowSelectionMenu("Choose one of options",
                    new string[] { "GetById", "Update", "Delete", "GetAll", "GetAllByUserRole", "Back" });

            switch (selection)
            {
                case "GetById":
                    await GetById();
                    break;
                case "Update":
                    await Update();
                    break;
                case "Delete":
                    await Delete();
                    break;
                case "GetAll":
                    await GetAll();
                    break;
                case "GetAllByUserRole":
                    await GetAllByUserRole();
                    break;
                case "Back":
                    circle = false;
                    break;
            }
        }
    }
}
