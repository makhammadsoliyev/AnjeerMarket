using AnjeerMarket.Models.Commons;

namespace AnjeerMarket.Models.Categories;

public class Category : Auditable
{
    public string Name { get; set; }
    public string Description { get; set; }
}