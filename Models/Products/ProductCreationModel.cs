namespace AnjeerMarket.Models.Products;

public class ProductCreationModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public long CategoryId { get; set; }
}
