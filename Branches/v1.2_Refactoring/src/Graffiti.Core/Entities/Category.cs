using System;
using System.Collections.Generic;
using System.Web;

namespace Graffiti.Core
{
    [Serializable]
    public class Category : IMenuItem
    {
        private CategoryCollection _children = new CategoryCollection();
        private object lockedObject = new object();
        private bool _loadedChildren = false;

        public int Id { get; set; }
        public string Name { get; set; }
        public string View { get; set; }
        public string PostView { get; set; }
        public string FormattedName { get; set; }
        public string LinkName { get; set; }
        public string FeedUrlOverride { get; set; }
        public string Body { get; set; }
        public string InitialCategoryName { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsLoaded { get; set; }

        private bool _isNew = true;
        public bool IsNew {
            get { return _isNew; }
            set { _isNew = value; }
        }

        public int PostCount { get; set; }
        public Guid UniqueId { get; set; }
        public int ParentId { get; set; }
        public int Type { get; set; }
        public string ImageUrl { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public int FeaturedId { get; set; }
        public int SortOrderTypeId { get; set; }
        public bool ExcludeSubCategoryPosts { get; set; }
 
        public SortOrderType SortOrder
        {
            get { return (SortOrderType) SortOrderTypeId; }
            set { SortOrderTypeId = (int) value; }
        }

        public bool IsUncategorized { get; set; }

        public CategoryCollection Children 
        { 
            get { return _children; } 
            set { _children = value; } 
        }

        public Category Parent { get; set; }

        /// <summary>
        /// Returns true if ParentId > 0
        /// </summary>
        public bool HasParent {
            get { return ParentId > 0; }
        }


        /// <summary>
        /// Returns true if ParentId > -1 and Children.Count > 0
        /// </summary>
        public bool HasChildren {
            get { return ParentId <= 0 && Children.Count > 0; }
        }

        /// <summary>
        /// Returns the absolute url of this category
        /// </summary>
        public string Url {
            get {
                return VirtualPathUtility.ToAbsolute(VirtualUrl);
            }
        }

        /// <summary>
        /// Returns the virtual url of this category (ie, ~/)
        /// </summary>
        public string VirtualUrl {
            get {
                return "~/" + LinkName + "/";
            }
        }

        public static bool IncludeChildPosts {
            get { return SiteSettings.Get().IncludeChildPosts; }
        }

        #region IMenuItem Members

        public List<MenuItem> GetMenuItems() {
            List<MenuItem> menuItems = new List<MenuItem>();

            menuItems.Add(new MenuItem("Id", "$category.Id", "The category id", "Category"));
            menuItems.Add(new MenuItem("Name", "$category.Name", "The proper name of the category", "Category"));
            menuItems.Add(new MenuItem("LinkName", "$category.LinkName", "The value used in the category url", "Category"));
            menuItems.Add(new MenuItem("FeedUrlOverride", "$category.FeedUrlOverride", "Optional value used direct requests to the category feed feedburner", "Category"));
            menuItems.Add(new MenuItem("Body", "$category.Body", "A description of the category", "Category"));
            menuItems.Add(new MenuItem("PostCount", "$category.PostCount", "Number of published posts in this category", "Category"));
            menuItems.Add(new MenuItem("UniqueId", "$category.UniqueId", "Global unique id (guid)", "Category"));
            menuItems.Add(new MenuItem("ParentId", "$category.ParentId", "Parent category (defaults to 0)", "Category"));
            menuItems.Add(new MenuItem("IsUncategorized", "$category.IsUncategorized", "Returns true if this category is the special uncategorized category", "Category"));
            menuItems.Add(new MenuItem("Children", "$category.Children", "A list of child categories", "Category"));
            menuItems.Add(new MenuItem("HasChildren", "$category.HasChildren", "Returns true if this category has child categories", "Category"));
            menuItems.Add(new MenuItem("Url", "$category.Url", "The absolute url of this category", "Category"));

            return menuItems;
        }

        #endregion

        #region event stuff

        //Local value containing the link name of the category. We compare this
        //value when saving changes to see if there has been a change
        //private string __initialCategoryName = null;

        //protected override void Loaded()
        //{
        //    base.Loaded();

        //    //Store the value in a local variable to check pre-save for changes
        //    __initialCategoryName = LinkName;
        //}

        //protected override void BeforeValidate()
        //{
        //    base.BeforeValidate();

        //    if (IsNew)
        //    {
        //        if(UniqueId == Guid.Empty)
        //            UniqueId = Guid.NewGuid();
        //    }

        //    if (string.IsNullOrEmpty(FormattedName))
        //        FormattedName = Util.CleanForUrl(Name);

        //    //We append the parent link name if it exists
        //    if (ParentId <= 0)
        //        LinkName = Util.CleanForUrl(FormattedName);
        //    else
        //        LinkName = new CategoryController().GetCachedCategory(ParentId, false).LinkName + "/" + Util.CleanForUrl(FormattedName);

        //    if(!Util.IsValidFileName(LinkName))
        //    {
        //        throw new Exception("Sorry, you cannot use the reserved word *" + LinkName + "* for a file name.");
        //    }

        //    if(string.IsNullOrEmpty(FeedUrlOverride))
        //        FeedUrlOverride = null;

        //    //We need to protected against category names colliding with uncategorized posts.
        //    //Uncategorized posts also do the same check
        //    if (Name != CategoryController.UncategorizedName)
        //    {
        //        Query q = Post.CreateQuery();
        //        q.AndWhere(Post.Columns.Name, LinkName);
        //        q.AndWhere(Post.Columns.CategoryId, CategoryController.UnCategorizedId);
        //        if (q.GetRecordCount() > 0)
        //            throw new Exception("Categories cannot have the same name as an uncategorized post");
        //    }

        //    //Check for changes. At this time, we do not support renaming 
        //    //categories if it has a post. 
        //    //if(__initialCategoryName != null && __initialCategoryName != LinkName)
        //    //{
        //    //    Query q = Post.CreateQuery();
        //    //    q.AddWhere(Post.Columns.CategoryId, Id);
        //    //    if (q.GetRecordCount() > 0)
        //    //        throw new Exception("Sorry, at this time you cannot rename a category with posts in it.");
        //    //}

        //    if (string.IsNullOrEmpty(Body) || Body == "<p>&nbsp;</p>")
        //        Body = null;
        //}




        //protected override void AfterCommit()
        //{
        //    base.AfterCommit();

        //    //Clear the cache
        //    CategoryController.Reset();

        //    //right pages so we can avoid UrlRewriting
        //    WritePages();
        //}

        #endregion

    }


}