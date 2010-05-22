using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using DataBuddy;
using Graffiti.Core;

namespace Graffiti.Data.DataBuddy
{
    [Serializable]
    public class DataCategory : DataBuddyBase
    {
        private static readonly Table _Table = null;

        static DataCategory() {
            _Table = new Table("graffiti_Categories", "Category");
            _Table.IsReadOnly = false;
            _Table.PrimaryKey = "Id";
            _Table.Columns.Add(new Column("Id", DbType.Int32, typeof(System.Int32), "Id", false, true));
            _Table.Columns.Add(new Column("Name", DbType.String, typeof(System.String), "Name", false, false));
            _Table.Columns.Add(new Column("View", DbType.String, typeof(System.String), "View", false, false));
            _Table.Columns.Add(new Column("PostView", DbType.String, typeof(System.String), "PostView", false, false));
            _Table.Columns.Add(new Column("FormattedName", DbType.String, typeof(System.String), "FormattedName", false, false));
            _Table.Columns.Add(new Column("LinkName", DbType.String, typeof(System.String), "LinkName", false, false));
            _Table.Columns.Add(new Column("FeedUrlOverride", DbType.String, typeof(System.String), "FeedUrlOverride", true, false));
            _Table.Columns.Add(new Column("Body", DbType.String, typeof(System.String), "Body", true, false));
            _Table.Columns.Add(new Column("IsDeleted", DbType.Boolean, typeof(System.Boolean), "IsDeleted", false, false));
            _Table.Columns.Add(new Column("Post_Count", DbType.Int32, typeof(System.Int32), "PostCount", false, false));
            _Table.Columns.Add(new Column("UniqueId", DbType.Guid, typeof(System.Guid), "UniqueId", false, false));
            _Table.Columns.Add(new Column("ParentId", DbType.Int32, typeof(System.Int32), "ParentId", false, false));
            _Table.Columns.Add(new Column("Type", DbType.Int32, typeof(System.Int32), "Type", false, false));
            _Table.Columns.Add(new Column("ImageUrl", DbType.String, typeof(System.String), "ImageUrl", true, false));
            _Table.Columns.Add(new Column("MetaDescription", DbType.String, typeof(System.String), "MetaDescription", true, false));
            _Table.Columns.Add(new Column("MetaKeywords", DbType.String, typeof(System.String), "MetaKeywords", true, false));
            _Table.Columns.Add(new Column("FeaturedId", DbType.Int32, typeof(System.Int32), "FeaturedId", false, false));
            _Table.Columns.Add(new Column("SortOrderTypeId", DbType.Int32, typeof(System.Int32), "SortOrderTypeId", false, false));
            _Table.Columns.Add(new Column("ExcludeSubCategoryPosts", DbType.Boolean, typeof(System.Boolean), "ExcludeSubCategoryPosts", false, false));
        }

        /// <summary>
        /// Fetches an instance of Category based on a single column value. If more than one record is found, only the first will be used.
        /// </summary>
        public static DataCategory FetchByColumn(Column column, object value) {
            Query q = new Query(_Table);
            q.AndWhere(column, value);
            return FetchByQuery(q);
        }

        public static DataCategory FetchByQuery(Query q) {
            DataCategory item = new DataCategory();
            using (IDataReader reader = q.ExecuteReader()) {
                if (reader.Read())
                    item.LoadAndCloseReader(reader);
            }

            return item;
        }

        /// <summary>
        /// Creates an instance of Query for the type Category
        /// </summary>
        public static Query CreateQuery() {
            return new Query(_Table);
        }

        public DataCategory() { }
        /// <summary>
        /// Loads an instance of Category for the supplied primary key value
        /// </summary>
        public DataCategory(object keyValue) {
            Query q = new Query(_Table);
            q.AndWhere(Columns.Id, keyValue);
            using (IDataReader reader = q.ExecuteReader()) {
                if (reader.Read())
                    LoadAndCloseReader(reader);
            }
        }

        /// <summary>
        /// Hydrates an instance of Category. In this case, the Reader should be in a read ready state. The reader will not be closed once it is done processing.
        /// </summary>
        public void Load(IDataReader reader) {
            Load(reader, false);
        }

        /// <summary>
        /// Hydrates an instance of Category. In this case, the Reader should be in a read ready state. The reader will be closed once it is done processing.
        /// </summary>
        public void LoadAndCloseReader(IDataReader reader) {
            Load(reader, true);
        }

        private void Load(IDataReader reader, bool close) {
            Id = DataService.GetValue<System.Int32>(Columns.Id, reader);
            Name = DataService.GetValue<System.String>(Columns.Name, reader);
            View = DataService.GetValue<System.String>(Columns.View, reader);
            PostView = DataService.GetValue<System.String>(Columns.PostView, reader);
            FormattedName = DataService.GetValue<System.String>(Columns.FormattedName, reader);
            LinkName = DataService.GetValue<System.String>(Columns.LinkName, reader);
            FeedUrlOverride = DataService.GetValue<System.String>(Columns.FeedUrlOverride, reader);
            Body = DataService.GetValue<System.String>(Columns.Body, reader);
            IsDeleted = DataService.GetValue<System.Boolean>(Columns.IsDeleted, reader);
            PostCount = DataService.GetValue<System.Int32>(Columns.PostCount, reader);
            UniqueId = DataService.GetValue<System.Guid>(Columns.UniqueId, reader);
            ParentId = DataService.GetValue<System.Int32>(Columns.ParentId, reader);
            Type = DataService.GetValue<System.Int32>(Columns.Type, reader);
            ImageUrl = DataService.GetValue<System.String>(Columns.ImageUrl, reader);
            MetaDescription = DataService.GetValue<System.String>(Columns.MetaDescription, reader);
            MetaKeywords = DataService.GetValue<System.String>(Columns.MetaKeywords, reader);
            FeaturedId = DataService.GetValue<System.Int32>(Columns.FeaturedId, reader);
            SortOrderTypeId = DataService.GetValue<System.Int32>(Columns.SortOrderTypeId, reader);
            ExcludeSubCategoryPosts = DataService.GetValue<System.Boolean>(Columns.ExcludeSubCategoryPosts, reader);
            Loaded();
            ResetStatus();

            if (close)
                reader.Close();
        }

        #region public System.Int32 Id

        private System.Int32 _Id;

        public System.Int32 Id {
            get { return _Id; }
            set { MarkDirty(); _Id = value; }
        }

        #endregion

        #region public System.String Name

        private System.String _Name;

        public System.String Name {
            get { return _Name; }
            set { MarkDirty(); _Name = value; }
        }

        #endregion

        #region public System.String View

        private System.String _View;

        public System.String View {
            get { return _View; }
            set { MarkDirty(); _View = value; }
        }

        #endregion

        #region public System.String PostView

        private System.String _PostView;

        public System.String PostView {
            get { return _PostView; }
            set { MarkDirty(); _PostView = value; }
        }

        #endregion

        #region public System.String FormattedName

        private System.String _FormattedName;

        public System.String FormattedName {
            get { return _FormattedName; }
            set { MarkDirty(); _FormattedName = value; }
        }

        #endregion

        #region public System.String LinkName

        private System.String _LinkName;

        public System.String LinkName {
            get { return _LinkName; }
            set { MarkDirty(); _LinkName = value; }
        }

        #endregion

        #region public System.String FeedUrlOverride

        private System.String _FeedUrlOverride;

        public System.String FeedUrlOverride {
            get { return _FeedUrlOverride; }
            set { MarkDirty(); _FeedUrlOverride = value; }
        }

        #endregion

        #region public System.String Body

        private System.String _Body;

        public System.String Body {
            get { return _Body; }
            set { MarkDirty(); _Body = value; }
        }

        #endregion

        #region public System.Boolean IsDeleted

        private System.Boolean _IsDeleted;

        public System.Boolean IsDeleted {
            get { return _IsDeleted; }
            set { MarkDirty(); _IsDeleted = value; }
        }

        #endregion

        #region public System.Int32 PostCount

        private System.Int32 _PostCount;

        public System.Int32 PostCount {
            get { return _PostCount; }
            set { MarkDirty(); _PostCount = value; }
        }

        #endregion

        #region public System.Guid UniqueId

        private System.Guid _UniqueId;

        public System.Guid UniqueId {
            get { return _UniqueId; }
            set { MarkDirty(); _UniqueId = value; }
        }

        #endregion

        #region public System.Int32 ParentId

        private System.Int32 _ParentId;

        public System.Int32 ParentId {
            get { return _ParentId; }
            set { MarkDirty(); _ParentId = value; }
        }

        #endregion

        #region public System.Int32 Type

        private System.Int32 _Type;

        public System.Int32 Type {
            get { return _Type; }
            set { MarkDirty(); _Type = value; }
        }

        #endregion

        #region public System.String ImageUrl

        private System.String _ImageUrl;

        public System.String ImageUrl {
            get { return _ImageUrl; }
            set { MarkDirty(); _ImageUrl = value; }
        }

        #endregion

        #region public System.String MetaDescription

        private System.String _MetaDescription;

        public System.String MetaDescription {
            get { return _MetaDescription; }
            set { MarkDirty(); _MetaDescription = value; }
        }

        #endregion

        #region public System.String MetaKeywords

        private System.String _MetaKeywords;

        public System.String MetaKeywords {
            get { return _MetaKeywords; }
            set { MarkDirty(); _MetaKeywords = value; }
        }

        #endregion

        #region public System.Int32 FeaturedId

        private System.Int32 _FeaturedId;

        public System.Int32 FeaturedId {
            get { return _FeaturedId; }
            set { MarkDirty(); _FeaturedId = value; }
        }

        #endregion

        #region public System.Int32 SortOrderTypeId

        private System.Int32 _SortOrderTypeId;

        public System.Int32 SortOrderTypeId {
            get { return _SortOrderTypeId; }
            set { MarkDirty(); _SortOrderTypeId = value; }
        }

        #endregion

        #region public System.Boolean ExcludeSubCategoryPosts

        private System.Boolean _ExcludeSubCategoryPosts;

        public System.Boolean ExcludeSubCategoryPosts {
            get { return _ExcludeSubCategoryPosts; }
            set { MarkDirty(); _ExcludeSubCategoryPosts = value; }
        }

        #endregion

        /// <summary>
        /// The table object which represents Category
        /// </summary>
        public static Table Table { get { return _Table; } }

        /// <summary>
        /// The columns which represent Category
        /// </summary>
        public static class Columns {
            public static readonly Column Id = new Column("Id", DbType.Int32, typeof(System.Int32), "Id", false, true);
            public static readonly Column Name = new Column("Name", DbType.String, typeof(System.String), "Name", false, false);
            public static readonly Column View = new Column("View", DbType.String, typeof(System.String), "View", false, false);
            public static readonly Column PostView = new Column("PostView", DbType.String, typeof(System.String), "PostView", false, false);
            public static readonly Column FormattedName = new Column("FormattedName", DbType.String, typeof(System.String), "FormattedName", false, false);
            public static readonly Column LinkName = new Column("LinkName", DbType.String, typeof(System.String), "LinkName", false, false);
            public static readonly Column FeedUrlOverride = new Column("FeedUrlOverride", DbType.String, typeof(System.String), "FeedUrlOverride", true, false);
            public static readonly Column Body = new Column("Body", DbType.String, typeof(System.String), "Body", true, false);
            public static readonly Column IsDeleted = new Column("IsDeleted", DbType.Boolean, typeof(System.Boolean), "IsDeleted", false, false);
            public static readonly Column PostCount = new Column("Post_Count", DbType.Int32, typeof(System.Int32), "PostCount", false, false);
            public static readonly Column UniqueId = new Column("UniqueId", DbType.Guid, typeof(System.Guid), "UniqueId", false, false);
            public static readonly Column ParentId = new Column("ParentId", DbType.Int32, typeof(System.Int32), "ParentId", false, false);
            public static readonly Column Type = new Column("Type", DbType.Int32, typeof(System.Int32), "Type", false, false);
            public static readonly Column ImageUrl = new Column("ImageUrl", DbType.String, typeof(System.String), "ImageUrl", true, false);
            public static readonly Column MetaDescription = new Column("MetaDescription", DbType.String, typeof(System.String), "MetaDescription", true, false);
            public static readonly Column MetaKeywords = new Column("MetaKeywords", DbType.String, typeof(System.String), "MetaKeywords", true, false);
            public static readonly Column FeaturedId = new Column("FeaturedId", DbType.Int32, typeof(System.Int32), "FeaturedId", false, false);
            public static readonly Column SortOrderTypeId = new Column("SortOrderTypeId", DbType.Int32, typeof(System.Int32), "SortOrderTypeId", false, false);
            public static readonly Column ExcludeSubCategoryPosts = new Column("ExcludeSubCategoryPosts", DbType.Boolean, typeof(System.Boolean), "ExcludeSubCategoryPosts", false, false);
        }

        public static int Delete(Column column, object value) {
            DataCategory objectToDelete = FetchByColumn(column, value);
            if (!objectToDelete.IsNew) {
                objectToDelete.BeforeRemove(false);
                int i = DataService.Delete(Table, column, value);
                objectToDelete.AfterRemove(false);
                return i;
            }

            return 0;
        }

        public static int Delete(object value) {
            return Delete(Columns.Id, value);
        }

        public static int Destroy(Column column, object value) {
            DataCategory objectToDelete = FetchByColumn(column, value);
            if (!objectToDelete.IsNew) {
                objectToDelete.BeforeRemove(true);
                int i = DataService.Destroy(Table, column, value);
                objectToDelete.AfterRemove(true);
                return i;
            }

            return 0;
        }


        public static int Destroy(object value) {
            return Destroy(Columns.Id, value);
        }
        protected override void SetPrimaryKey(int pkID) {
            Id = pkID;
        }

        protected override Table GetTable() {
            return Table;
        }

        public static Parameter FindParameter(List<Parameter> parameters, string name) {
            if (parameters == null)
                throw new ArgumentNullException("parameters");
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name", "The value cannot be null or an empty string.");

            return parameters.Find(delegate(Parameter p) { return (p.Name == name); });
        }

        public static Parameter FindParameter(string name) {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name", "The value cannot be null or an empty string.");

            return GenerateParameters().Find(delegate(Parameter p) { return (p.Name == name); });
        }

        #region public static List<Parameter> GenerateParameters()

        public static List<Parameter> GenerateParameters() {
            List<Parameter> parameters = new List<Parameter>(Table.Columns.Count);

            parameters.Add(new Parameter("Id", null, DbType.Int32));

            Parameter pName = new Parameter("Name", null, DbType.String);
            pName.Length = 255;
            parameters.Add(pName);

            Parameter pView = new Parameter("View", null, DbType.String);
            pView.Length = 64;
            parameters.Add(pView);

            Parameter pPostView = new Parameter("PostView", null, DbType.String);
            pPostView.Length = 64;
            parameters.Add(pPostView);

            Parameter pFormattedName = new Parameter("FormattedName", null, DbType.String);
            pFormattedName.Length = 255;
            parameters.Add(pFormattedName);

            Parameter pLinkName = new Parameter("LinkName", null, DbType.String);
            pLinkName.Length = 255;
            parameters.Add(pLinkName);

            Parameter pFeedUrlOverride = new Parameter("FeedUrlOverride", null, DbType.String);
            pFeedUrlOverride.Length = 255;
            parameters.Add(pFeedUrlOverride);

            Parameter pBody = new Parameter("Body", null, DbType.String);
            pBody.Length = 2147483647;
            parameters.Add(pBody);

            parameters.Add(new Parameter("IsDeleted", null, DbType.Boolean));

            parameters.Add(new Parameter("Post_Count", null, DbType.Int32));

            parameters.Add(new Parameter("UniqueId", null, DbType.Guid));

            parameters.Add(new Parameter("ParentId", null, DbType.Int32));

            parameters.Add(new Parameter("Type", null, DbType.Int32));

            Parameter pImageUrl = new Parameter("ImageUrl", null, DbType.String);
            pImageUrl.Length = 255;
            parameters.Add(pImageUrl);

            Parameter pMetaDescription = new Parameter("MetaDescription", null, DbType.String);
            pMetaDescription.Length = 255;
            parameters.Add(pMetaDescription);

            Parameter pMetaKeywords = new Parameter("MetaKeywords", null, DbType.String);
            pMetaKeywords.Length = 255;
            parameters.Add(pMetaKeywords);

            parameters.Add(new Parameter("FeaturedId", null, DbType.Int32));

            parameters.Add(new Parameter("SortOrderTypeId", null, DbType.Int32));

            parameters.Add(new Parameter("ExcludeSubCategoryPosts", null, DbType.Boolean));

            return parameters;
        }

        #endregion

        protected override List<Parameter> GetParameters() {
            List<Parameter> parameters = new List<Parameter>(Table.Columns.Count);

            if (!IsNew) {
                parameters.Add(new Parameter("Id", Id, DbType.Int32));
            }

            Parameter pName = new Parameter("Name", Name, DbType.String);
            pName.Length = 255;
            parameters.Add(pName);

            Parameter pView = new Parameter("View", View, DbType.String);
            pView.Length = 64;
            parameters.Add(pView);

            Parameter pPostView = new Parameter("PostView", PostView, DbType.String);
            pPostView.Length = 64;
            parameters.Add(pPostView);

            Parameter pFormattedName = new Parameter("FormattedName", FormattedName, DbType.String);
            pFormattedName.Length = 255;
            parameters.Add(pFormattedName);

            Parameter pLinkName = new Parameter("LinkName", LinkName, DbType.String);
            pLinkName.Length = 255;
            parameters.Add(pLinkName);

            Parameter pFeedUrlOverride = new Parameter("FeedUrlOverride", FeedUrlOverride, DbType.String);
            pFeedUrlOverride.Length = 255;
            parameters.Add(pFeedUrlOverride);

            Parameter pBody = new Parameter("Body", Body, DbType.String);
            pBody.Length = 2147483647;
            parameters.Add(pBody);

            parameters.Add(new Parameter("IsDeleted", IsDeleted, DbType.Boolean));

            parameters.Add(new Parameter("Post_Count", PostCount, DbType.Int32));

            parameters.Add(new Parameter("UniqueId", UniqueId, DbType.Guid));

            parameters.Add(new Parameter("ParentId", ParentId, DbType.Int32));

            parameters.Add(new Parameter("Type", Type, DbType.Int32));

            Parameter pImageUrl = new Parameter("ImageUrl", ImageUrl, DbType.String);
            pImageUrl.Length = 255;
            parameters.Add(pImageUrl);

            Parameter pMetaDescription = new Parameter("MetaDescription", MetaDescription, DbType.String);
            pMetaDescription.Length = 255;
            parameters.Add(pMetaDescription);

            Parameter pMetaKeywords = new Parameter("MetaKeywords", MetaKeywords, DbType.String);
            pMetaKeywords.Length = 255;
            parameters.Add(pMetaKeywords);

            parameters.Add(new Parameter("FeaturedId", FeaturedId, DbType.Int32));

            parameters.Add(new Parameter("SortOrderTypeId", SortOrderTypeId, DbType.Int32));

            parameters.Add(new Parameter("ExcludeSubCategoryPosts", ExcludeSubCategoryPosts, DbType.Boolean));

            return parameters;
        }

        public SortOrderType SortOrder
        {
            get { return (SortOrderType) SortOrderTypeId; }
            set { SortOrderTypeId = (int) value; }
        }

        //Local value containing the link name of the category. We compare this
        //value when saving changes to see if there has been a change
        private string __initialCategoryName = null;

        protected override void Loaded()
        {
            base.Loaded();

            //Store the value in a local variable to check pre-save for changes
            __initialCategoryName = LinkName;
        }

        protected override void BeforeValidate()
        {
            base.BeforeValidate();

            if (IsNew)
            {
                if(UniqueId == Guid.Empty)
                    UniqueId = Guid.NewGuid();
            }

            if (string.IsNullOrEmpty(FormattedName))
                FormattedName = DataUtil.CleanForUrl(Name);

            //We append the parent link name if it exists
            if (ParentId <= 0)
                LinkName = DataUtil.CleanForUrl(FormattedName);
            else
                LinkName = new DataCategoryController().GetCachedCategory(ParentId, false).LinkName + "/" + DataUtil.CleanForUrl(FormattedName);

            if(!DataUtil.IsValidFileName(LinkName))
            {
                throw new Exception("Sorry, you cannot use the reserved word *" + LinkName + "* for a file name.");
            }

            if(string.IsNullOrEmpty(FeedUrlOverride))
                FeedUrlOverride = null;

            //We need to protected against category names colliding with uncategorized posts.
            //Uncategorized posts also do the same check
            if (Name != DataCategoryController.UncategorizedName)
            {
                Query q = DataPost.CreateQuery();
                q.AndWhere(DataPost.Columns.Name, LinkName);
                q.AndWhere(DataPost.Columns.CategoryId, DataCategoryController.UnCategorizedId);
                if (q.GetRecordCount() > 0)
                    throw new Exception("Categories cannot have the same name as an uncategorized post");
            }

            //Check for changes. At this time, we do not support renaming 
            //categories if it has a post. 
            //if(__initialCategoryName != null && __initialCategoryName != LinkName)
            //{
            //    Query q = Post.CreateQuery();
            //    q.AddWhere(Post.Columns.CategoryId, Id);
            //    if (q.GetRecordCount() > 0)
            //        throw new Exception("Sorry, at this time you cannot rename a category with posts in it.");
            //}

            if (string.IsNullOrEmpty(Body) || Body == "<p>&nbsp;</p>")
                Body = null;
        }

        protected override void AfterCommit()
        {
            base.AfterCommit();

            //Clear the cache
            DataCategoryController.Reset();

            //write pages so we can avoid UrlRewriting
            WritePages();
        }

        /// <summary>
        /// Returns true if the Id of this category matches CategoryController.UnCategorizedId
        /// </summary>
        public bool IsUncategorized
        {
            get { return Id == DataCategoryController.UnCategorizedId; }
        }

        /// <summary>
        /// Writes empty pages to disk. 
        /// </summary>
        public void WritePages()
        {
            PageTemplateToolboxContext templateContext = new PageTemplateToolboxContext();
            templateContext.Put("CategoryId", Id);
            templateContext.Put("CategoryName", LinkName);
            templateContext.Put("MetaDescription",
                                !string.IsNullOrEmpty(MetaDescription)
                                    ? MetaDescription
                                    : HttpUtility.HtmlEncode(DataUtil.RemoveHtml(Body, 255) ?? string.Empty));

            templateContext.Put("MetaKeywords",
                                !string.IsNullOrEmpty(MetaKeywords)
                                    ? MetaKeywords
                                    : Name);



            if (!IsUncategorized) {
                PageWriter.Write("category.view", "~/" + LinkName + "/" + DataUtil.DEFAULT_PAGE, templateContext);
                PageWriter.Write("categoryrss.view", "~/" + LinkName + "/feed/" + DataUtil.DEFAULT_PAGE, templateContext);

            }

            if (__initialCategoryName != null && __initialCategoryName != LinkName) {
                DataPostCollection pc = new DataPostCollection();
                Query postQuery = DataPost.CreateQuery();
                postQuery.AndWhere(DataPost.Columns.CategoryId, Id);
                pc.LoadAndCloseReader(postQuery.ExecuteReader());
                foreach (DataPost p in pc) {
                    p.Save();
                }
            }
        }

        #region Properties

        private DataCategoryCollection _children = new DataCategoryCollection();
        private object lockedObject = new object();

        private bool _loadedChildren = false;

        /// <summary>
        /// Returns the sub categories if they exist. 
        /// </summary>
        public DataCategoryCollection Children
        {
            get
            {
                if(!_loadedChildren)
                {
                    lock (lockedObject)
                    {
                        if (!_loadedChildren)
                        {
                            foreach (DataCategory c in new DataCategoryController().GetCachedCategories())
                            {
                                if (c.ParentId == Id)
                                    _children.Add(c);
                            }

                            _children.Sort(delegate(DataCategory c1, DataCategory c2)
                                               {
                                                   return
                                                       Comparer<string>.Default.Compare(c1.Name,
                                                                                        c2.Name);
                                               });
                        }
                        _loadedChildren = true;
                    }
                }
                 return _children;
            }

        }

        /// <summary>
        /// Returns the parent Category object
        /// </summary>
        public DataCategory Parent
        {
            get 
            {
                return new DataCategoryController().GetCachedCategory(this.ParentId, true);
            }
        }

        /// <summary>
        /// Returns true if ParentId > 0
        /// </summary>
        public bool HasParent
        {
            get { return ParentId > 0; }
        }


        /// <summary>
        /// Returns true if ParentId > -1 and Children.Count > 0
        /// </summary>
        public bool HasChildren
        {
            get { return ParentId <= 0 && Children.Count > 0; }
        }

        /// <summary>
        /// Returns the absolute url of this category
        /// </summary>
        public string Url
        {
            get
            {
                return VirtualPathUtility.ToAbsolute(VirtualUrl);
            }
        }

        /// <summary>
        /// Returns the virtual url of this category (ie, ~/)
        /// </summary>
        public string VirtualUrl
        {
            get
            {
                return "~/" + LinkName + "/";
            }
        }

        #endregion

        public static bool IncludeChildPosts
        {
            get {
                return SiteSettings.Get().IncludeChildPosts; 
            }
        }

    }

    public class DataCategoryController
    {

        public static void UpdatePostCounts()
        {
            if (!DataUtil.IsAccess)
            {
                DataService.ExecuteNonQuery(
                    new QueryCommand(
                        "Update graffiti_Categories Set Post_Count = (Select " + DataService.Provider.SqlCountFunction() + " FROM graffiti_Posts where graffiti_Posts.CategoryId = graffiti_Categories.Id and graffiti_Posts.Status = 1 and graffiti_Posts.IsPublished = 1 graffiti_Posts.IsDeleted = 0)"));
                DataService.ExecuteNonQuery(
                    new QueryCommand(
                        "Update graffiti_Categories Set Post_Count = (Select coalesce(sum(x.Post_Count), 0) FROM " + DataService.Provider.GenerateDerivedView("graffiti_Categories") + " AS x where x.ParentId = graffiti_Categories.Id) + Post_Count Where graffiti_Categories.ParentId <= 0"));
            }
            else
            {
                QueryCommand cmd1 = new QueryCommand("select count(*) AS PostCount, p.CategoryId from graffiti_Posts p where p.Status = 1 and p.IsPublished = @IsPublished and p.IsDeleted = 0 group by p.CategoryId");
				cmd1.Parameters.Add("IsPublished", true);

				using(IDataReader dr = DataService.ExecuteReader(cmd1))
				{
					while ( dr.Read() )
					{
						QueryCommand uCmd = new QueryCommand(string.Format("update graffiti_Categories set Post_Count = {0} where Id = {1}", dr["PostCount"], dr["CategoryId"]));
						DataService.ExecuteNonQuery(uCmd);
					}
				}

				QueryCommand cmd = new QueryCommand("Select sum(x.Post_Count) as ParentIdPostCount, x.ParentId FROM graffiti_Categories AS x where x.ParentId > 0 group by x.ParentId");
				using (IDataReader dr = DataService.ExecuteReader(cmd))
				{
					while (dr.Read())
					{
						QueryCommand uCmd = new QueryCommand(string.Format("update graffiti_Categories set Post_Count = Post_Count + {0} where Id = {1} and ParentId <= 0", dr["ParentIdPostCount"], dr["ParentId"]));
						DataService.ExecuteNonQuery(uCmd);
					}
				}
			}
        }

        public static readonly string CacheKey = "graffiti-categories";
        public static readonly string UncategorizedName = "Uncategorized";

        /// <summary>
        /// Returns all the parent categories and the UnCategorized Categry
        /// </summary>
        /// <returns></returns>
        public DataCategoryCollection GetAllTopLevelCachedCategories()
        {
            return GetTopLevelCachedCategories(false);
        }

        /// <summary>
        /// Returns the all the parent categories, but excludes the uncateogirzed category
        /// </summary>
        /// <returns></returns>
        public DataCategoryCollection GetTopLevelCachedCategories()
        {
            return GetTopLevelCachedCategories(true);
        }


        private DataCategoryCollection GetTopLevelCachedCategories(bool filterUncategorized)
        {
            DataCategoryCollection cc = new DataCategoryCollection();
            foreach(DataCategory c in GetAllCachedCategories())
            {
                if(c.ParentId <= 0 )
                {
                    if (filterUncategorized && c.Name == UncategorizedName)
                        continue;

                    cc.Add(c);
                }
            }

            cc.Sort(delegate(DataCategory c1, DataCategory c2) { return Comparer<string>.Default.Compare(c1.Name, c2.Name); });

            return cc;
        }

        /// <summary>
        /// Returns all categories along with the uncategorized category
        /// </summary>
        /// <returns></returns>
        public DataCategoryCollection GetAllCachedCategories()
        {
            DataCategoryCollection cc = DataCategoryCollection.FetchAll();
                
            bool foundUncategorized = false;
            foreach (DataCategory c in cc)
            {
                if (c.Name == UncategorizedName)
                {
                    foundUncategorized = true;
                    break;
                }
            }

            if (!foundUncategorized)
            {
                DataCategory uncategorizedCategory = new DataCategory();
                uncategorizedCategory.Name = UncategorizedName;
                uncategorizedCategory.LinkName = "uncategorized";
                uncategorizedCategory.Save();
                cc.Add(uncategorizedCategory);

            }

            return cc;

        }

        /// <summary>
        /// Returns all categories except the uncategorized category
        /// </summary>
        /// <returns></returns>
        public DataCategoryCollection GetCachedCategories()
        {
            DataCategoryCollection cc = GetAllCachedCategories();
            DataCategoryCollection cc2 = new DataCategoryCollection();
            foreach (DataCategory c in cc)
                if (c.Name != UncategorizedName)
                    cc2.Add(c);


            cc2.Sort(delegate(DataCategory c1, DataCategory c2) { return Comparer<string>.Default.Compare(c1.Name, c2.Name); });


            return cc2;
        }

        /// <summary>
        /// Returns a single category.
        /// </summary>
        /// <param name="id">Id of the category to return</param>
        /// <param name="allowNull">determins if an exception is thrown if the category does not exist</param>
        /// <returns></returns>
        public DataCategory GetCachedCategory(int id, bool allowNull)
        {
            DataCategoryCollection cc = GetAllCachedCategories();
            foreach (DataCategory c in cc)
                if (c.Id == id)
                    return c;

            if (allowNull)
                return null;
            else
                throw new Exception("The category " + id + " could not be found");
        }

        /// <summary>
        /// Returns a single category.
        /// </summary>
        /// <param name="name">The name of the category to find. This is not case sensitive</param>
        /// <param name="allowNull">determins if an exception is thrown if the category does not exist</param>
        /// <returns></returns>
        public DataCategory GetCachedCategory(string name, bool allowNull)
        {
            DataCategoryCollection cc = GetAllCachedCategories();
            foreach (DataCategory c in cc)
                if (DataUtil.AreEqualIgnoreCase(c.Name, name))
                    return c;

            if (allowNull)
                return null;
            else
                throw new Exception("The category " + name + " could not be found");
        }

        /// <summary>
        /// Returns a single category.
        /// </summary>
        /// <param name="name">The linkname of the category to find. This is not case sensitive</param>
        /// <param name="allowNull">determins if an exception is thrown if the category does not exist</param>
        /// <returns></returns>
        public DataCategory GetCachedCategoryByLinkName(string linkName, bool allowNull)
        {
            DataCategoryCollection cc = GetAllCachedCategories();
            foreach (DataCategory c in cc)
                if (DataUtil.AreEqualIgnoreCase(c.LinkName, linkName))
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
		  public DataCategory GetCachedCategoryByLinkName(string linkName, int ParentId, bool allowNull)
		  {
			  DataCategoryCollection cc = GetAllCachedCategories();
			  foreach (DataCategory c in cc)
				  if (c.ParentId == ParentId && DataUtil.AreEqualIgnoreCase(c.LinkName, linkName))
					  return c;

			  if (allowNull)
				  return null;
			  else
				  throw new Exception("The category with link name " + linkName + " could not be found");
		  }

        /// <summary>
        /// Returns the special uncategorized category (which does exist in the db)
        /// </summary>
        /// <returns></returns>
        public DataCategory GetUnCategorizedCategory()
        {
            return GetCachedCategory(UncategorizedName, false);
        }

        /// <summary>
        /// Clears the category cache
        /// </summary>
        public static void Reset()
        {
            //ZCache.RemoveCache(CacheKey);
        }

        private static int _uncatId = -1;

        /// <summary>
        /// Returns the Id of the Uncategorized Category. This value is cached staticly. 
        /// </summary>
        public static int UnCategorizedId
        {
            get
            {
                if (_uncatId == -1)
                {
                    _uncatId = new DataCategoryController().GetUnCategorizedCategory().Id;
                }

                return _uncatId;
            }
        }
    }

    public enum SortOrderType
    {
        Ascending = 1,
        Descending = 0,
        Views = 2,
        Custom = 3,
        Alphabetical = 4,

    }
}