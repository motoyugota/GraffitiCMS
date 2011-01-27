using System;
using System.Linq;
using Graffiti.Core;

namespace Graffiti.Data 
{
    public interface ICategoryRepository 
    {
        IQueryable<Category> FetchAllCategories();
        Category FetchCategory(object id);
        Category FetchCategoryByUniqueId(Guid uniqueId);
        Category SaveCategory(Category category);
        Category SaveCategory(Category category, string username);
        void DestroyCategory(int categoryId);
    }
}
