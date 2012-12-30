using System;
using System.Collections.Generic;

namespace Graffiti.Core.Services 
{
    public interface ICategoryService 
    {
        int UnCategorizedId();
        string UncategorizedName();
        Category FetchCategory(object id);
        Category FetchCachedCategory(int id, bool allowNull);
        Category FetchCachedCategory(string name, bool allowNull);
        Category FetchCachedCategoryByLinkName(string categoryName, bool allowNull);
        Category FetchCachedCategoryByLinkName(string linkName, int parentId, bool allowNull);
        Category FetchCategoryByUniqueId(Guid uniqueId);
        Category FetchUnCategorizedCategory();
        IList<Category> FetchTopLevelCachedCategories();
        IList<Category> FetchAllTopLevelCachedCategories();
        IList<Category> FetchAllCachedCategories();
        IList<Category> FetchCachedCategories();
        Category SaveCategory(Category category);
        Category SaveCategory(Category category, string username);
        void DestroyCategory(int categoryId);
        void WriteCategoryPages(Category category);
        void Reset();
    }



}
