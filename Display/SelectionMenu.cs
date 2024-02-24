using AnjeerMarket.Models.Categories;
using AnjeerMarket.Models.OrderItems;
using AnjeerMarket.Models.Products;
using AnjeerMarket.Models.Users;
using Spectre.Console;

namespace AnjeerMarket.Display;

public class SelectionMenu
{
    public Table DataTable(string title, params OrderItemViewModel[] orderItems)
    {
        var table = new Table();

        table.Title(title.ToUpper())
            .BorderColor(Color.Blue)
            .AsciiBorder();

        table.AddColumn("ID");
        table.AddColumn("Product");
        table.AddColumn("Quantity");
        table.AddColumn("Date");

        foreach (var orderItem in orderItems)
            table.AddRow(orderItem.Id.ToString(), orderItem.Product.Name, orderItem.Quantity.ToString(), orderItem.Order.Date.ToString());

        table.Border = TableBorder.Rounded;
        table.Centered();

        return table;
    }

    public Table DataTable(string title, params ProductViewModel[] products)
    {
        var table = new Table();

        table.Title(title.ToUpper())
            .BorderColor(Color.Blue)
            .AsciiBorder();

        table.AddColumn("ID");
        table.AddColumn("Name");
        table.AddColumn("Description");
        table.AddColumn("Price");
        table.AddColumn("Category");

        foreach (var product in products)
            table.AddRow(product.Id.ToString(), product.Name, product.Description, product.Price.ToString(), product.Category.Name);

        table.Border = TableBorder.Rounded;
        table.Centered();

        return table;
    }

    public Table DataTable(string title, params CategoryViewModel[] categories)
    {
        var table = new Table();

        table.Title(title.ToUpper())
            .BorderColor(Color.Blue)
            .AsciiBorder();

        table.AddColumn("ID");
        table.AddColumn("Name");
        table.AddColumn("Description");

        foreach (var user in categories)
            table.AddRow(user.Id.ToString(), user.Name, user.Description);

        table.Border = TableBorder.Rounded;
        table.Centered();

        return table;
    }

    public Table DataTable(string title, params UserViewModel[] users)
    {
        var table = new Table();

        table.Title(title.ToUpper())
            .BorderColor(Color.Blue)
            .AsciiBorder();

        table.AddColumn("ID");
        table.AddColumn("FirstName");
        table.AddColumn("LastName");
        table.AddColumn("Email");
        table.AddColumn("Role");

        foreach (var user in users)
            table.AddRow(user.Id.ToString(), user.FirstName, user.LastName, user.Email, user.Role.ToString());

        table.Border = TableBorder.Rounded;
        table.Centered();

        return table;
    }

    public string ShowSelectionMenu(string title, string[] options)
    {
        var selection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .PageSize(5) // Number of items visible at once
                .AddChoices(options)
                .HighlightStyle(new Style(foreground: Color.Cyan1, background: Color.Blue))
        );

        return selection;
    }
}
