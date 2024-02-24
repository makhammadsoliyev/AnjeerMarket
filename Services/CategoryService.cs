﻿using AnjeerMarket.Configurations;
using AnjeerMarket.Extensions;
using AnjeerMarket.Helpers;
using AnjeerMarket.Interfaces;
using AnjeerMarket.Models.Categories;

namespace AnjeerMarket.Services;

public class CategoryService : ICategoryService
{
    private List<Category> categories;

    public async Task<CategoryViewModel> CreateAsync(CategoryCreationModel category)
    {
        categories = await FileIO.ReadAsync<Category>(Constants.CATEGORIES_PATH);
        var createdCategory = category.MapTo<Category>();
        createdCategory.Id = categories.GenerateId();

        categories.Add(createdCategory);

        await FileIO.WriteAsync(Constants.CATEGORIES_PATH, categories);

        return createdCategory.MapTo<CategoryViewModel>();
    }

    public async Task<bool> DeleteAsync(long id)
    {
        categories = await FileIO.ReadAsync<Category>(Constants.CATEGORIES_PATH);
        var category = categories.FirstOrDefault(c => !c.IsDeleted && c.Id == id)
            ?? throw new Exception($"Category was not found with this id: {id}");

        category.IsDeleted = true;
        category.DeletedAt = DateTime.UtcNow;

        await FileIO.WriteAsync(Constants.CATEGORIES_PATH, categories);

        return true;
    }

    public async Task<IEnumerable<CategoryViewModel>> GetAllAsync()
    {
        categories = await FileIO.ReadAsync<Category>(Constants.CATEGORIES_PATH);
        return categories.FindAll(c => !c.IsDeleted).Select(c => c.MapTo<CategoryViewModel>());
    }

    public async Task<CategoryViewModel> GetByIdAsync(long id)
    {
        categories = await FileIO.ReadAsync<Category>(Constants.CATEGORIES_PATH);
        var category = categories.FirstOrDefault(c => !c.IsDeleted && c.Id == id)
            ?? throw new Exception($"Category was not found with this id: {id}");

        return category.MapTo<CategoryViewModel>();
    }

    public async Task<CategoryViewModel> UpdateAsync(long id, CategoryUpdateModel category)
    {
        categories = await FileIO.ReadAsync<Category>(Constants.CATEGORIES_PATH);
        var existCategory = categories.FirstOrDefault(c => !c.IsDeleted && c.Id == id)
            ?? throw new Exception($"Category was not found with this id: {id}");

        existCategory.Id = id;
        existCategory.Name = category.Name;
        existCategory.UpdatedAt = DateTime.UtcNow;
        existCategory.Description = category.Description;

        await FileIO.WriteAsync(Constants.CATEGORIES_PATH, categories);

        return existCategory.MapTo<CategoryViewModel>();
    }
}
