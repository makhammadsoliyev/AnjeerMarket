using AnjeerMarket.Enums;
using AnjeerMarket.Interfaces;
using AnjeerMarket.Models.Categories;
using AnjeerMarket.Models.Products;
using AnjeerMarket.Models.Users;
using AnjeerMarket.Services;
using Spectre.Console;

namespace AnjeerMarket.Display;

public class ProductMenu
{
    private IEnumerable<ProductViewModel> products;
    private readonly SelectionMenu selectionMenu;
    private readonly IProductService productService;
    private readonly ICategoryService categoryService;

    public ProductMenu(IProductService productService, ICategoryService categoryService)
    {
        this.productService = productService;
        this.selectionMenu = new SelectionMenu();
        this.categoryService = categoryService;
    }

    private async Task Add()
    {
        string name = AnsiConsole.Ask<string>("[blue]Name: [/]");
        string description = AnsiConsole.Ask<string>("[cyan2]Description: [/]");
        decimal price = AnsiConsole.Ask<decimal>("[yellow]Price: [/]");
        while (price <= 0)
        {
            AnsiConsole.MarkupLine($"[red]Invalid input.[/]");
            price = AnsiConsole.Ask<long>("[yellow]Price: [/]");
        }
        var categories = await categoryService.GetAllAsync();
        var selection = selectionMenu.ShowSelectionMenu("Categories", categories.Select(u => $"{u.Id} {u.Name}").ToArray());
        var categoryId = Convert.ToInt64(selection.Split()[0]);

        var product = new ProductCreationModel()
        {
            Name = name,
            Price = price,
            CategoryId = categoryId,
            Description = description,
        };


        try
        {
            var addedProduct = await productService.CreateAsync(product);
            var table = new SelectionMenu().DataTable("Product", addedProduct);
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

    private async Task GetById()
    {
        products = await productService.GetAllAsync();
        var selection = selectionMenu.ShowSelectionMenu("Products", products.Select(p => $"{p.Id} {p.Name}").ToArray());
        var productId = Convert.ToInt64(selection.Split()[0]);

        try
        {
            var category = await categoryService.GetByIdAsync(productId);
            var table = new SelectionMenu().DataTable("Category", category);
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
        products = await productService.GetAllAsync();
        var selection = selectionMenu.ShowSelectionMenu("Products", products.Select(p => $"{p.Id} {p.Name}").ToArray());
        var productId = Convert.ToInt64(selection.Split()[0]);
        string name = AnsiConsole.Ask<string>("[blue]Name: [/]");
        string description = AnsiConsole.Ask<string>("[cyan2]Description: [/]");
        decimal price = AnsiConsole.Ask<decimal>("[yellow]Price: [/]");
        while (price <= 0)
        {
            AnsiConsole.MarkupLine($"[red]Invalid input.[/]");
            price = AnsiConsole.Ask<long>("[yellow]Price: [/]");
        }
        var categories = await categoryService.GetAllAsync();
        var selectionCategories = selectionMenu.ShowSelectionMenu("Categories", categories.Select(u => $"{u.Id} {u.Name}").ToArray());
        var categoryId = Convert.ToInt64(selectionCategories.Split()[0]);

        var product = new ProductUpdateModel()
        {
            Name = name,
            Price = price,
            CategoryId = categoryId,
            Description = description,
        };


        try
        {
            var updatedProduct = await productService.UpdateAsync(productId, product);
            var table = new SelectionMenu().DataTable("Product", updatedProduct);
            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine("[blue]Enter to continue...[/]");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        }
        Thread.Sleep(1500);
    }

    private async Task Delete()
    {
        products = await productService.GetAllAsync();
        var selection = selectionMenu.ShowSelectionMenu("Products", products.Select(p => $"{p.Id} {p.Name}").ToArray());
        var productId = Convert.ToInt64(selection.Split()[0]);

        try
        {
            bool isDeleted = await categoryService.DeleteAsync(productId);
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
        products = await productService.GetAllAsync();
        var table = new SelectionMenu().DataTable("Products", products.ToArray());
        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("[blue]Enter to continue...[/]");
        Console.ReadKey();
    }

    private async Task GetAllByCategory()
    {
        var categories = await categoryService.GetAllAsync();
        var selection = selectionMenu.ShowSelectionMenu("Categories", categories.Select(u => $"{u.Id} {u.Name}").ToArray());
        var categoryId = Convert.ToInt64(selection.Split()[0]);
        products = await productService.GetAllAsync(categoryId);
        var table = new SelectionMenu().DataTable("Products", products.ToArray());
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
                new string[] { "Add", "GetById", "Update", "Delete", "GetAll", "GetAllByCategory", "Back" });

            switch (selection)
            {
                case "Add":
                    await Add();
                    break;
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
                case "GetAllByCategory":
                    await GetAllByCategory();
                    break;
                case "Back":
                    circle = false;
                    break;
            }
        }
    }
}
