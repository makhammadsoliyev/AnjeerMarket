using AnjeerMarket.Configurations;
using AnjeerMarket.Extensions;
using AnjeerMarket.Helpers;
using AnjeerMarket.Interfaces;
using AnjeerMarket.Models.Products;

namespace AnjeerMarket.Services;

public class ProductService : IProductService
{
    private List<Product> products;
    private readonly ICategoryService categoryService;

    public ProductService(ICategoryService categoryService)
    {
        this.categoryService = categoryService;
    }

    public async Task<ProductViewModel> CreateAsync(ProductCreationModel product)
    {
        var category = await categoryService.GetByIdAsync(product.CategoryId);
        products = await FileIO.ReadAsync<Product>(Constants.PRODUCTS_PATH);

        var createdProduct = product.MapTo<Product>();
        createdProduct.Id = products.GenerateId();

        products.Add(createdProduct);

        await FileIO.WriteAsync(Constants.PRODUCTS_PATH, products);

        var res = createdProduct.MapTo<ProductViewModel>();
        res.Category = category;

        return res;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        products = await FileIO.ReadAsync<Product>(Constants.PRODUCTS_PATH);
        var product = products.FirstOrDefault(p => p.Id == id && !p.IsDeleted)
            ?? throw new Exception($"Product was not found with this id = {id}");

        product.IsDeleted = true;
        product.DeletedAt = DateTime.UtcNow;

        await FileIO.WriteAsync(Constants.PRODUCTS_PATH, products);

        return true;
    }

    public async Task<IEnumerable<ProductViewModel>> GetAllAsync(long? categoryId = null)
    {
        products = await FileIO.ReadAsync<Product>(Constants.PRODUCTS_PATH);
        products = products.FindAll(p => !p.IsDeleted);

        var result = new List<ProductViewModel>();
        foreach (var product in products)
        {
            var category = await categoryService.GetByIdAsync(product.CategoryId);
            var p = product.MapTo<ProductViewModel>();
            p.Category = category;

            result.Add(p);
        }

        return result;
    }

    public async Task<ProductViewModel> GetByIdAsync(long id)
    {
        products = await FileIO.ReadAsync<Product>(Constants.PRODUCTS_PATH);
        var product = products.FirstOrDefault(p => p.Id == id && !p.IsDeleted)
            ?? throw new Exception($"Product was not found with this id = {id}");

        var category = await categoryService.GetByIdAsync(product.CategoryId);
        var res = product.MapTo<ProductViewModel>();
        res.Category = category;

        return res;
    }

    public async Task<ProductViewModel> UpdateAsync(long id, ProductUpdateModel product)
    {
        products = await FileIO.ReadAsync<Product>(Constants.PRODUCTS_PATH);
        var existProduct = products.FirstOrDefault(p => p.Id == id && !p.IsDeleted)
            ?? throw new Exception($"Product was not found with this id = {id}");
        var category = await categoryService.GetByIdAsync(product.CategoryId);

        existProduct.Name = existProduct.Name;
        existProduct.Price = existProduct.Price;
        existProduct.UpdatedAt = DateTime.UtcNow;
        existProduct.CategoryId = product.CategoryId;
        existProduct.Description = existProduct.Description;

        await FileIO.WriteAsync(Constants.PRODUCTS_PATH, products);

        var res = existProduct.MapTo<ProductViewModel>();
        res.Category = category;

        return res;
    }
}
