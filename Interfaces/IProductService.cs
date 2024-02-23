using AnjeerMarket.Models.Products;

namespace AnjeerMarket.Interfaces;

public interface IProductService
{
    Task<ProductViewModel> CreateAsync(ProductCreationModel product);
    Task<ProductViewModel> GetByIdAsync(long id);
    Task<ProductViewModel> UpdateAsync(long id, ProductUpdateModel product);
    Task<bool> DeleteAsync(long id);
    Task<IEnumerable<ProductViewModel>> GetAllAsync(long? categoryId = null);
}
