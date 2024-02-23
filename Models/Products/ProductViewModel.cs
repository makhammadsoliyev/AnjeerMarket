using AnjeerMarket.Models.Categories;

namespace AnjeerMarket.Models.Products;

public class ProductViewModel
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public CategoryViewModel Category { get; set; }
}