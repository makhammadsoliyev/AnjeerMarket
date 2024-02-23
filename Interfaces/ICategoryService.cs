using AnjeerMarket.Models.Categories;

namespace AnjeerMarket.Interfaces;

public interface ICategoryService
{
    Task<CategoryViewModel> CreateAsync(CategoryCreationModel category);
    Task<CategoryViewModel> GetByIdAsync(long id);
    Task<CategoryViewModel> UpdateAsync(long id, CategoryUpdateModel category);
    Task<bool> DeleteAsync(long id);
    Task<IEnumerable<CategoryViewModel>> GetAllAsync();
}
