using System.Collections.Generic;
using System.Linq;

namespace Graffiti.Core
{
    public class CategoryPage : TemplatedThemePage
    {

        protected override string ViewLookUp(string baseName, string defaultViewName)
        {
            Category category = _categoryService.FetchCachedCategory(CategoryID, false);

            // category-name.view
            if (category.ParentId <= 0 && ViewExists(CategoryName + baseName))
                return CategoryName + baseName;

            // Subcategories
            if (category.ParentId > 0)
            {
                // parent-name.child-name.view
                if (ViewExists(category.LinkName.Replace("/", ".") + baseName))
                    return category.LinkName.Replace("/", ".") + baseName;

                // childcategory.parent-name.view
                if (ViewExists("childcategory." + category.Parent.LinkName + baseName))
                    return "childcategory." + category.Parent.LinkName + baseName;

                // childcategory.view
                if (ViewExists("childcategory" + baseName))
                    return "childcategory" + baseName;

                // parent-name.view
                if (ViewExists(category.Parent.LinkName + baseName))
                    return category.Parent.LinkName + baseName;
            }

            // category.view
            if (ViewExists("category" + baseName))
                return "category" + baseName;
            
            // index.view
            return base.ViewLookUp(baseName, defaultViewName);

        }

        protected override void LoadContent(GraffitiContext graffitiContext)
        {
            IsIndexable = false;

            graffitiContext["where"] = "category";

            Category category = _categoryService.FetchCachedCategory(CategoryID, false);

            if(CategoryName != null && !Util.AreEqualIgnoreCase(CategoryName,category.LinkName))
            {
                RedirectTo(category.Url);
            }

            graffitiContext["title"] = category.Name + " : " + SiteSettings.Get().Title;
            graffitiContext["category"] = category;

            graffitiContext.RegisterOnRequestDelegate("posts", GetCategoryPosts);
           
            // GetCategoryPosts needs to be called so the pager works
            GetCategoryPosts("posts", graffitiContext);
        }

        protected virtual object GetCategoryPosts(string key, GraffitiContext graffitiContext)
        {
            Category category = _categoryService.FetchCachedCategory(CategoryID, false);
            int pageSize = SiteSettings.Get().PageSize;
            PostCollection pc = ZCache.Get<PostCollection>(string.Format(CacheKey, PageIndex, CategoryID, category.SortOrder, pageSize));
            if (pc == null)
            {
                pc = new PostCollection(_postService.FetchPosts(pageSize, category.SortOrder));

                if (Category.IncludeChildPosts)
                {
                    if (category.ParentId > 0)
                        pc = new PostCollection(pc.Where(x => x.CategoryId == CategoryID).ToList());
                    else
                    {
                        List<int> ids = new List<int>(category.Children.Count + 1);
                        foreach (Category child in category.Children)
                            ids.Add(child.Id);

                        ids.Add(category.Id);

                        pc = new PostCollection(pc.Where(x => ids.Contains(x.CategoryId)).ToList());
                    }
                }
                else
                {
                    pc = new PostCollection(pc.Where(x => x.CategoryId == CategoryID).ToList());
                }
                ZCache.InsertCache(string.Format(CacheKey, PageIndex, CategoryID, category.SortOrder, pageSize), pc, 60);
            }

            graffitiContext.TotalRecords = category.PostCount;
            graffitiContext.PageIndex = PageIndex;
            graffitiContext.PageSize = SiteSettings.Get().PageSize;

            return pc;
        }

        private const string CacheKey = "Posts-Categories-P:{0}-C:{1}-T:{2}-PS:{3}";
    }
}