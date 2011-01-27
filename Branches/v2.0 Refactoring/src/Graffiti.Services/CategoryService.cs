
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Graffiti.Core;
using Graffiti.Core.Services;
using Graffiti.Data;

namespace Graffiti.Services 
{
    public class CategoryService : ICategoryService
    {
        private static readonly string CacheKey = "graffiti-categories";
        private static readonly string _uncategorizedName = "Uncategorized";

        private ICategoryRepository _categoryRepository;
        private IPostRepository _postRepository;

        public CategoryService(ICategoryRepository categoryRepository, IPostRepository postRepository)
        {
            _categoryRepository = categoryRepository;
            _postRepository = postRepository;
        }

        public Category FetchCategory(object id)
        {
            return _categoryRepository.FetchCategory(id);
        }

        public Category FetchCategoryByUniqueId(Guid uniqueId)
        {
            return _categoryRepository.FetchCategoryByUniqueId(uniqueId);
        }

        public void DestroyCategory(int categoryId)
        {
            _categoryRepository.DestroyCategory(categoryId);
        }

        /// <summary>
        /// Returns the special uncategorized category (which does exist in the db)
        /// </summary>
        /// <returns></returns>
        public Category FetchUnCategorizedCategory()
        {
            return FetchCachedCategory(UncategorizedName(), false);
        }

        public string UncategorizedName()
        {
            return _uncategorizedName;
        }

        /// <summary>
        /// Clears the category cache
        /// </summary>
        public void Reset()
        {
            ZCache.RemoveCache(CacheKey);
        }

        private static int _uncatId = -1;

        /// <summary>
        /// Returns the Id of the Uncategorized Category. This value is cached staticly. 
        /// </summary>
        public int UnCategorizedId()
        {
            if (_uncatId == -1)
            {
                _uncatId = FetchUnCategorizedCategory().Id;
            }
            return _uncatId;
        }

        /// <summary>
        /// Returns a single category.
        /// </summary>
        /// <param name="id">Id of the category to return</param>
        /// <param name="allowNull">determins if an exception is thrown if the category does not exist</param>
        /// <returns></returns>
        public Category FetchCachedCategory(int id, bool allowNull) {
            
            Category c = FetchAllCachedCategories().Where(x => x.Id == id).FirstOrDefault();

            if (c != null)
                return c;

            if (allowNull)
                return null;
            else
                throw new Exception("The category " + id + " could not be found");
        }

        /// <summary>
        /// Returns all categories along with the uncategorized category
        /// </summary>
        /// <returns></returns>
        public IList<Category> FetchAllCachedCategories() 
        {
            IList<Category> cc = ZCache.Get<CategoryCollection>(CacheKey);
            if (cc == null) {
                cc = _categoryRepository.FetchAllCategories().ToList();

                bool foundUncategorized = false;
                foreach (Category c in cc) {
                    if (c.Name == UncategorizedName()) {
                        foundUncategorized = true;
                        break;
                    }
                }

                if (!foundUncategorized) {
                    Category uncategorizedCategory = new Category();
                    uncategorizedCategory.Name = UncategorizedName();
                    uncategorizedCategory.LinkName = "uncategorized";
                    SaveCategory(uncategorizedCategory);
                    cc.Add(uncategorizedCategory);

                }

                ZCache.InsertCache(CacheKey, cc, 90);
            }
            return cc;
        }


        /// <summary>
        /// Returns a single category.
        /// </summary>
        /// <param name="name">The name of the category to find. This is not case sensitive</param>
        /// <param name="allowNull">determins if an exception is thrown if the category does not exist</param>
        /// <returns></returns>
        public Category FetchCachedCategory(string name, bool allowNull) 
        {
            Category c = FetchAllCachedCategories().Where(x => Util.AreEqualIgnoreCase(x.Name, name)).FirstOrDefault();
            if (c != null)
                return c;

            if (allowNull)
                return null;
            else
                throw new Exception("The category " + name + " could not be found");
        }

        /// <summary>
        /// Returns all the parent categories and the UnCategorized Categry
        /// </summary>
        /// <returns></returns>
        public IList<Category> FetchAllTopLevelCachedCategories() {
            return FetchTopLevelCachedCategories(false);
        }

        /// <summary>
        /// Returns the all the parent categories, but excludes the uncateogirzed category
        /// </summary>
        /// <returns></returns>
        public IList<Category> FetchTopLevelCachedCategories() {
            return FetchTopLevelCachedCategories(true);
        }


        private IList<Category> FetchTopLevelCachedCategories(bool filterUncategorized) 
        {
            IList<Category> cc = new CategoryCollection();
            foreach (Category c in FetchAllCachedCategories()) {
                if (c.ParentId <= 0) {
                    if (filterUncategorized && c.Name == UncategorizedName())
                        continue;
                    cc.Add(c);
                }
            }

            cc.OrderBy(x => x.Name);

            return cc;
        }

        /// <summary>
        /// Returns a single category.
        /// </summary>
        /// <param name="name">The linkname of the category to find. This is not case sensitive</param>
        /// <param name="allowNull">determins if an exception is thrown if the category does not exist</param>
        /// <returns></returns>
        public Category FetchCachedCategoryByLinkName(string linkName, bool allowNull) 
        {
            Category c = FetchAllCachedCategories().Where(x => Util.AreEqualIgnoreCase(x.LinkName, linkName)).FirstOrDefault();
            if (c != null)
                return c;

            if (allowNull)
                return null;
            else
                throw new Exception("The category with link name " + linkName + " could not be found");
        }

        /// <summary>
        /// Returns a single category.
        /// </summary>
        /// <param name="name">The linkname of the category to find. This is not case sensitive</param>
        /// <param name="allowNull">determins if an exception is thrown if the category does not exist</param>
        /// <returns></returns>
        public Category FetchCachedCategoryByLinkName(string linkName, int parentId, bool allowNull) 
        {
            Category c = FetchAllCachedCategories().Where(x => Util.AreEqualIgnoreCase(x.LinkName, linkName) && x.ParentId == parentId).FirstOrDefault();
            if (c != null)
                return c;

            if (allowNull)
                return null;
            else
                throw new Exception("The category with link name " + linkName + " could not be found");
        }

        /// <summary>
        /// Returns all categories except the uncategorized category
        /// </summary>
        /// <returns></returns>
        public IList<Category> FetchCachedCategories() 
        {
            IList<Category> cc = FetchAllCachedCategories();
            IList<Category> cc2 = new List<Category>();
            foreach (Category c in cc)
                if (c.Name != UncategorizedName())
                    cc2.Add(c);

            cc2.OrderBy(x => x.Name);

            return cc2;
        }

        public Category SaveCategory(Category category) 
        {
            return _categoryRepository.SaveCategory(category);
        }

        public Category SaveCategory(Category category, string username)
        {
            return _categoryRepository.SaveCategory(category, username);
        }

        /// <summary>
        /// Writes empty pages to disk. 
        /// </summary>
        public void WriteCategoryPages(Category category) 
        {
            PageTemplateToolboxContext templateContext = new PageTemplateToolboxContext();
            templateContext.Put("CategoryId", category.Id);
            templateContext.Put("CategoryName", category.LinkName);
            templateContext.Put("MetaDescription",
                                !string.IsNullOrEmpty(category.MetaDescription)
                                    ? category.MetaDescription
                                    : HttpUtility.HtmlEncode(Util.RemoveHtml(category.Body, 255) ?? string.Empty));

            templateContext.Put("MetaKeywords",
                                !string.IsNullOrEmpty(category.MetaKeywords)
                                    ? category.MetaKeywords
                                    : category.Name);



            if (!category.IsUncategorized) {
                PageWriter.Write("category.view", "~/" + category.LinkName + "/" + Util.DEFAULT_PAGE, templateContext);
                PageWriter.Write("categoryrss.view", "~/" + category.LinkName + "/feed/" + Util.DEFAULT_PAGE, templateContext);
            }

            if (category.InitialCategoryName != null && category.InitialCategoryName != category.LinkName) 
            {
                foreach (Post p in _postRepository.FetchPosts().Where(x => x.CategoryId == category.Id)) 
                {
                    _postRepository.SavePost(p);
                }
            }
        }


        //public partial class CategoryController
        //{

        //    public static void UpdatePostCounts()
        //    {
        //        if (!Util.IsAccess)
        //        {
        //            DataService.ExecuteNonQuery(
        //                new QueryCommand(
        //                    "Update graffiti_Categories Set Post_Count = (Select " + DataService.Provider.SqlCountFunction() + " FROM graffiti_Posts where graffiti_Posts.CategoryId = graffiti_Categories.Id and graffiti_Posts.Status = 1 and graffiti_Posts.IsPublished = 1)"));
        //            DataService.ExecuteNonQuery(
        //                new QueryCommand(
        //                    "Update graffiti_Categories Set Post_Count = (Select coalesce(sum(x.Post_Count), 0) FROM " + DataService.Provider.GenerateDerivedView("graffiti_Categories") + " AS x where x.ParentId = graffiti_Categories.Id) + Post_Count Where graffiti_Categories.ParentId <= 0"));
        //        }
        //        else
        //        {
        //            QueryCommand cmd1 = new QueryCommand("select count(*) AS PostCount, p.CategoryId from graffiti_Posts p where p.Status = 1 and p.IsPublished = @IsPublished group by p.CategoryId");
        //            cmd1.Parameters.Add("IsPublished", true);

        //            using(IDataReader dr = DataService.ExecuteReader(cmd1))
        //            {
        //                while ( dr.Read() )
        //                {
        //                    QueryCommand uCmd = new QueryCommand(string.Format("update graffiti_Categories set Post_Count = {0} where Id = {1}", dr["PostCount"], dr["CategoryId"]));
        //                    DataService.ExecuteNonQuery(uCmd);
        //                }
        //            }

        //            QueryCommand cmd = new QueryCommand("Select sum(x.Post_Count) as ParentIdPostCount, x.ParentId FROM graffiti_Categories AS x where x.ParentId > 0 group by x.ParentId");
        //            using (IDataReader dr = DataService.ExecuteReader(cmd))
        //            {
        //                while (dr.Read())
        //                {
        //                    QueryCommand uCmd = new QueryCommand(string.Format("update graffiti_Categories set Post_Count = Post_Count + {0} where Id = {1} and ParentId <= 0", dr["ParentIdPostCount"], dr["ParentId"]));
        //                    DataService.ExecuteNonQuery(uCmd);
        //                }
        //            }
        //        }
        //    }
    }
}
