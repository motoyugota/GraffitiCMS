using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using System.Web;
using Graffiti.Core.Services;


namespace Graffiti.Core
{
    [Serializable]
    public class Post : IMenuItem
    {
        private ICategoryService _categoryService;

        public Post(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public Post()
        {
            _categoryService = ServiceLocator.Get<ICategoryService>();
        }

		public int Id { get; set; }
        public string Title { get; set; }
        public string PostBody { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int Status { get; set; }
        public string ContentType { get; set; }
        public string Name { get; set; }

        private int _commentCount = 0;
        public int CommentCount
        {
            get { return _commentCount;} 
            set { _commentCount = value; }
        }

        public string TagList { get; set; }
        public int CategoryId { get; set; }
        public int Version { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public string ExtendedBody { get; set; }
        public bool IsLoaded { get; set; }

        private bool _isNew = true;
        public bool IsNew 
        {
            get { return _isNew; }
            set { _isNew = value; }
        }

        public bool IsDeleted { get; set; }
        public DateTime Published { get; set; }
        public int PendingCommentCount { get; set; }
        public int Views { get; set; }
        public Guid UniqueId { get; set; }
        public bool EnableComments { get; set; }
        public string PropertyKeys { get; set; }
        public string PropertyValues { get; set; }
        public string UserName { get; set; }
        public string Notes { get; set; }
        public string ImageUrl { get; set; }
		public string MetaDescription { get; set; }
    	public string MetaKeywords { get; set; }
		public bool IsPublished { get; set; }
        public int SortOrder { get; set; }
        public int ParentId { get; set; }
        public bool IsHome { get; set; }
        public int HomeSortOrder { get; set; }

        #region Properties

        /// <summary>
        /// User of the current post. This will cause an exception if the UserName value is null
        /// </summary>
        public IGraffitiUser User {
            get {
                return GraffitiUsers.GetUser(UserName);
            }
        }

        /// <summary>
        /// The proper name of the user of the current post. If the user does not exist, will return "deleted user"
        /// </summary>
        public string UserProperName {
            get {
                if (!string.IsNullOrEmpty(UserName)) {
                    IGraffitiUser u = GraffitiUsers.GetUser(UserName);
                    if (u != null)
                        return u.ProperName;
                }
                return "deleted user";
            }
        }

        public string ResolvedImageUrl {
            get {
                if (ImageUrl != null) {
                    if (ImageUrl.StartsWith("~/"))
                        return VirtualPathUtility.ToAbsolute(ImageUrl);
                    return ImageUrl.Trim();
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Flag used to make sure posts returned via search results are not committed to the database
        /// </summary>
        public bool IsCreatedBySearch = false;

        /// <summary>
        /// The current category of the post. 
        /// </summary>
        public Category Category {
            get { return _categoryService.FetchCachedCategory(CategoryId, false); }
        }

        /// <summary>
        /// Combines both the PostBody and ExtendedBody properties. This should be used when you
        /// wish to show the full post
        /// </summary>
        public string Body {
            get {
                StringBuilder sb = new StringBuilder();
                sb.Append(PostBody);
                sb.Append(ExtendedBody);

                // Allow plugins to append aditional content to the post body
                Events.Instance().ExecuteRenderPostBody(sb, this, PostRenderLocation.Web);

                return sb.ToString();
            }
        }

        /// <summary>
        /// Combines both the PostBody and ExtendedBody properties. This should be used by RSS feeds and
        /// other non-html locations. It will show the full post.
        /// </summary>
        public string RenderBody(PostRenderLocation renderLocation) {
            StringBuilder sb = new StringBuilder();
            sb.Append(PostBody);
            sb.Append(ExtendedBody);

            // Allow plugins to append aditional content to the post body
            Events.Instance().ExecuteRenderPostBody(sb, this, renderLocation);

            return sb.ToString();
        }

		  /// <summary>
		  /// Returns the Post excerpt. If a PostBody exists, it will be used.
		  /// If not, it returns the first 300 non-HTML characters
		  /// </summary>
		  public string Excerpt(string startText, string endText, string linkText, int len)
		  {
            string link = "";

            if (!String.IsNullOrEmpty(linkText))
                link = string.Format("{0}<a href=\"{1}\">{2}</a>{3}", startText, Url, linkText, endText);

            if (PostBody.Length <= len && String.IsNullOrEmpty(ExtendedBody))
                return "<p>" + Util.RemoveHtml(PostBody, len) + "</p>";
            else
                return "<p>" + Util.RemoveHtml(PostBody, len) + "...</p>" + link;
        }


        /// <summary>
        /// Returns the first len none HTML chacters of the PostBody property
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public string CustomExcerpt(int len) {
            return Util.RemoveHtml(PostBody, len);
        }

        /// <summary>
        /// Helper access to the Status int.
        /// </summary>
        public PostStatus PostStatus {
            get { return (PostStatus)Status; }
            set { Status = (int)value; }
        }

        /// <summary>
        /// Returns absolute URL to the post.
        /// </summary>
        public string Url {
            get {
                return VirtualPathUtility.ToAbsolute(VirtualUrl);
            }
        }

        /// <summary>
        /// Returns absolute URL to the post.
        /// </summary>
        public string RevisionUrl {
            get {
                return VirtualPathUtility.ToAbsolute(VirtualUrl) + "?revision=" + Version;
            }
        }

        /// <summary>
        /// Returns the virtual URL to the post.
        /// </summary>
        public string VirtualUrl {
            get {
                if (_categoryService.UnCategorizedId() != CategoryId)
                    return "~/" + _categoryService.FetchCachedCategory(CategoryId, false).LinkName + "/" + Name + "/";
                else
                    return "~/" + Name + "/";
            }
        }

        /// <summary>
        /// Returns True if CommentCount > 0. There maybe unapproved comments
        /// </summary>
        public bool HasComments { get { return CommentCount > 0; } }

        /// <summary>
        /// Returns True if this post will allow new comments
        /// </summary>
        public bool EnableNewComments { get { return CommentSettings.Get().EnableCommentOnPost(this); } }

        #endregion

        #region IMenuItem Members

        public List<MenuItem> GetMenuItems() {
            List<MenuItem> menuItems = new List<MenuItem>();

            menuItems.Add(new MenuItem("User", "$post.User", "Graffiti User", "Post"));
            menuItems.Add(new MenuItem("Category", "$post.Category", "Graffiti Category", "Post"));
            menuItems.Add(new MenuItem("Body", "$post.Body", "Renders PostBody + ExtendedBody", "Post"));
            menuItems.Add(new MenuItem("Excerpt", "$post.Excerpt", "If ExtendedBody is not null, it renders PostBody. Otherwise, it will render the first 200 non-HTML characters from PostBody", "Post"));
            menuItems.Add(new MenuItem("Url", "$post.Url", "Absolute url to the post", "Post"));
            menuItems.Add(new MenuItem("HasComments", "$post.HasComments", "Returns true if a post has one or more comment", "Post"));
            menuItems.Add(new MenuItem("EnableNewComments", "$post.EnableNewComments", "Returns true if the post allows new comments", "Post"));
            menuItems.Add(new MenuItem("Id", "$post.Id", "The post id", "Post"));
            menuItems.Add(new MenuItem("Title", "$post.Title", "The title of the post", "Post"));
            menuItems.Add(new MenuItem("PostBody", "$post.PostBody", "The main post content - this can be \'lead-in\' or excerpt if ExtendedBody is specified", "Post"));
            menuItems.Add(new MenuItem("Name", "$post.Name", "The value used in the post\'s url", "Post"));
            menuItems.Add(new MenuItem("CommentCount", "$post.CommentCount", "Number of published comments", "Post"));
            menuItems.Add(new MenuItem("TagList", "$post.TagList", "Comma seperated list of tags", "Post"));
            menuItems.Add(new MenuItem("Version", "$post.Version", "Version number of the post", "Post"));
            menuItems.Add(new MenuItem("ExtendedBody", "$post.ExtendedBody", "Optional post content - this is usually displayed when the full post view is requested", "Post"));
            menuItems.Add(new MenuItem("PendingCommentCount", "$post.PendingCommentCount ", "Number of comments which require approval", "Post"));
            menuItems.Add(new MenuItem("Views", "$post.Views", "Number of times the post has been viewed (note: we do try to prevent against refreshes)", "Post"));
            menuItems.Add(new MenuItem("UniqueId", "$post.UniqueId", "Globally unique id for the post (guid)", "Post"));
            menuItems.Add(new MenuItem("EnableComments", "$post.EnableComments", "Are comments enabled for this post", "Post"));
            menuItems.Add(new MenuItem("UserName", "$post.UserName", "Username of the post\'s author", "Post"));

            return menuItems;
        }

        #endregion

        public static Post FromXML(string xml) 
        {
            Post the_Post = ObjectManager.ConvertToObject<Post>(xml);
            //the_Post.Loaded();
            //the_Post.ResetStatus();

            return the_Post;
        }

        public string ToXML() {
            SerializeCustomFields();
            return ObjectManager.ConvertToString(this);
        }

        #region Custom Field Support

        //container for custom field values
        private NameValueCollection _nvc = new NameValueCollection();
        private bool _isCustomReady = false;

        /// <summary>
        /// Returns a custom fiels keys and values
        /// </summary>
        /// <returns></returns>
        public NameValueCollection CustomFields() {
            if (!_isCustomReady)
                DeserializeCustomFields();
            return _nvc;
        }

        /// <summary>
        /// Called Pre-Update to set the PropertyKeys and PropertyValues properties
        /// </summary>
        internal void SerializeCustomFields() {
            StringBuilder sbKey = new StringBuilder();
            StringBuilder sbValue = new StringBuilder();

            int index = 0;
            foreach (string key in _nvc.AllKeys) {
                if (key.IndexOf(':') != -1)
                    throw new ArgumentException("Extened Fields Key can not contain the character \":\"");

                string v = _nvc[key];
                if (!string.IsNullOrEmpty(v)) {
                    sbKey.AppendFormat("{0}:S:{1}:{2}:", key, index, v.Length);
                    sbValue.Append(v);
                    index += v.Length;
                }
            }


            PropertyKeys = sbKey.ToString();
            PropertyValues = sbValue.ToString();
        }

        /// <summary>
        /// Called during Loaded() method to copy values from PropertyKeys and PropertyValues 
        /// to the CustomFields NameValueCollection
        /// </summary>
        internal void DeserializeCustomFields() {
            _nvc.Clear();

            if (PropertyKeys != null && PropertyValues != null) {
                char[] splitter = new char[1] { ':' };
                string[] keyNames = PropertyKeys.Split(splitter);

                for (int i = 0; i < (keyNames.Length / 4); i++) {
                    int start = int.Parse(keyNames[(i * 4) + 2], CultureInfo.InvariantCulture);
                    int len = int.Parse(keyNames[(i * 4) + 3], CultureInfo.InvariantCulture);
                    string key = keyNames[i * 4];

                    if (((keyNames[(i * 4) + 1] == "S") && (start >= 0)) && (len > 0) && (PropertyValues.Length >= (start + len))) {
                        _nvc[key] = PropertyValues.Substring(start, len);
                    }
                }
            }

            _isCustomReady = true;
        }

        /// <summary>
        /// Provides access to the custom fields collection
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key] {
            get { return _nvc[key]; }
            set { _nvc[key] = value; }
        }

        /// <summary>
        /// Returns the custom field value for the given key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <example>
        /// #foreach($post in $posts)
        /// [TAB]The value of my custom field is $post.Custom("MyCustomFieldName")
        /// #end
        /// </example>
        public string Custom(string key) {
            return this[key];
        }

        #endregion
    }    
}