using AnjeerMarket.Enums;
using AnjeerMarket.Interfaces;
using AnjeerMarket.Models.Users;
using Spectre.Console;
using System.Text.RegularExpressions;

namespace AnjeerMarket.Display;

public class RegistrationMenu
{
    private readonly IUserService userService;

    public RegistrationMenu(IUserService userService)
    {
        this.userService = userService;
    }

    public async Task Add()
    {
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

        var user = new UserCreationModel()
        {
            Email = email,
            Password = password,
            LastName = lastName,
            FirstName = firstName,
            Role = Enum.Parse<Role>(role),
        };

        try
        {
            var addedUser = await userService.CreateAsync(user);
            AnsiConsole.MarkupLine("[green]Successfully added...[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        }
        Thread.Sleep(1500);
    }
}
