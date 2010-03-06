using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using DataBuddy;
using Graffiti.Core;

namespace Graffiti.Data.DataBuddy
{
    [Serializable]
    public class DataComment : DataBuddyBase
    {

        private static readonly Table _Table = null;

        static DataComment() {
            _Table = new Table("graffiti_Comments", "Comment");
            _Table.IsReadOnly = false;
            _Table.PrimaryKey = "Id";
            _Table.Columns.Add(new Column("Id", DbType.Int32, typeof(System.Int32), "Id", false, true));
            _Table.Columns.Add(new Column("PostId", DbType.Int32, typeof(System.Int32), "PostId", false, false));
            _Table.Columns.Add(new Column("Body", DbType.String, typeof(System.String), "Body", false, false));
            _Table.Columns.Add(new Column("CreatedBy", DbType.String, typeof(System.String), "CreatedBy", true, false));
            _Table.Columns.Add(new Column("Published", DbType.DateTime, typeof(System.DateTime), "Published", false, false));
            _Table.Columns.Add(new Column("Name", DbType.String, typeof(System.String), "Name", false, false));
            _Table.Columns.Add(new Column("IsPublished", DbType.Boolean, typeof(System.Boolean), "IsPublished", false, false));
            _Table.Columns.Add(new Column("Version", DbType.Int32, typeof(System.Int32), "Version", false, false));
            _Table.Columns.Add(new Column("WebSite", DbType.String, typeof(System.String), "WebSite", true, false));
            _Table.Columns.Add(new Column("SpamScore", DbType.Int32, typeof(System.Int32), "SpamScore", false, false));
            _Table.Columns.Add(new Column("IPAddress", DbType.String, typeof(System.String), "IPAddress", true, false));
            _Table.Columns.Add(new Column("ModifiedOn", DbType.DateTime, typeof(System.DateTime), "ModifiedOn", false, false));
            _Table.Columns.Add(new Column("ModifiedBy", DbType.String, typeof(System.String), "ModifiedBy", true, false));
            _Table.Columns.Add(new Column("Email", DbType.String, typeof(System.String), "Email", true, false));
            _Table.Columns.Add(new Column("IsDeleted", DbType.Boolean, typeof(System.Boolean), "IsDeleted", false, false));
            _Table.Columns.Add(new Column("UniqueId", DbType.Guid, typeof(System.Guid), "UniqueId", false, false));
            _Table.Columns.Add(new Column("UserName", DbType.String, typeof(System.String), "UserName", true, false));
            _Table.Columns.Add(new Column("IsTrackback", DbType.Boolean, typeof(System.Boolean), "IsTrackback", false, false));
        }

        /// <summary>
        /// Fetches an instance of Comment based on a single column value. If more than one record is found, only the first will be used.
        /// </summary>
        public static DataComment FetchByColumn(Column column, object value) {
            Query q = new Query(_Table);
            q.AndWhere(column, value);
            return FetchByQuery(q);
        }

        public static DataComment FetchByQuery(Query q) {
            DataComment item = new DataComment();
            using (IDataReader reader = q.ExecuteReader()) {
                if (reader.Read())
                    item.LoadAndCloseReader(reader);
            }

            return item;
        }

        /// <summary>
        /// Creates an instance of Query for the type Comment
        /// </summary>
        public static Query CreateQuery() {
            return new Query(_Table);
        }

        public DataComment() { }
        /// <summary>
        /// Loads an instance of Comment for the supplied primary key value
        /// </summary>
        public DataComment(object keyValue) {
            Query q = new Query(_Table);
            q.AndWhere(Columns.Id, keyValue);
            using (IDataReader reader = q.ExecuteReader()) {
                if (reader.Read())
                    LoadAndCloseReader(reader);
            }
        }

        /// <summary>
        /// Hydrates an instance of Comment. In this case, the Reader should be in a read ready state. The reader will not be closed once it is done processing.
        /// </summary>
        public void Load(IDataReader reader) {
            Load(reader, false);
        }

        /// <summary>
        /// Hydrates an instance of Comment. In this case, the Reader should be in a read ready state. The reader will be closed once it is done processing.
        /// </summary>
        public void LoadAndCloseReader(IDataReader reader) {
            Load(reader, true);
        }

        private void Load(IDataReader reader, bool close) {
            Id = DataService.GetValue<System.Int32>(Columns.Id, reader);
            PostId = DataService.GetValue<System.Int32>(Columns.PostId, reader);
            Body = DataService.GetValue<System.String>(Columns.Body, reader);
            CreatedBy = DataService.GetValue<System.String>(Columns.CreatedBy, reader);
            Published = DataService.GetValue<System.DateTime>(Columns.Published, reader);
            Name = DataService.GetValue<System.String>(Columns.Name, reader);
            IsPublished = DataService.GetValue<System.Boolean>(Columns.IsPublished, reader);
            Version = DataService.GetValue<System.Int32>(Columns.Version, reader);
            WebSite = DataService.GetValue<System.String>(Columns.WebSite, reader);
            SpamScore = DataService.GetValue<System.Int32>(Columns.SpamScore, reader);
            IPAddress = DataService.GetValue<System.String>(Columns.IPAddress, reader);
            ModifiedOn = DataService.GetValue<System.DateTime>(Columns.ModifiedOn, reader);
            ModifiedBy = DataService.GetValue<System.String>(Columns.ModifiedBy, reader);
            Email = DataService.GetValue<System.String>(Columns.Email, reader);
            IsDeleted = DataService.GetValue<System.Boolean>(Columns.IsDeleted, reader);
            UniqueId = DataService.GetValue<System.Guid>(Columns.UniqueId, reader);
            UserName = DataService.GetValue<System.String>(Columns.UserName, reader);
            IsTrackback = DataService.GetValue<System.Boolean>(Columns.IsTrackback, reader);
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

        #region public System.Int32 PostId

        private System.Int32 _PostId;

        public System.Int32 PostId {
            get { return _PostId; }
            set { MarkDirty(); _PostId = value; }
        }

        #endregion

        #region public System.String Body

        private System.String _Body;

        public System.String Body {
            get { return _Body; }
            set { MarkDirty(); _Body = value; }
        }

        #endregion

        #region public System.String CreatedBy

        private System.String _CreatedBy;

        public System.String CreatedBy {
            get { return _CreatedBy; }
            set { MarkDirty(); _CreatedBy = value; }
        }

        #endregion

        #region public System.DateTime Published

        private System.DateTime _Published;

        public System.DateTime Published {
            get { return _Published; }
            set { MarkDirty(); _Published = value; }
        }

        #endregion

        #region public System.String Name

        private System.String _Name;

        public System.String Name {
            get { return _Name; }
            set { MarkDirty(); _Name = value; }
        }

        #endregion

        #region public System.Boolean IsPublished

        private System.Boolean _IsPublished;

        public System.Boolean IsPublished {
            get { return _IsPublished; }
            set { MarkDirty(); _IsPublished = value; }
        }

        #endregion

        #region public System.Int32 Version

        private System.Int32 _Version;

        public System.Int32 Version {
            get { return _Version; }
            set { MarkDirty(); _Version = value; }
        }

        #endregion

        #region public System.String WebSite

        private System.String _WebSite;

        public System.String WebSite {
            get { return _WebSite; }
            set { MarkDirty(); _WebSite = value; }
        }

        #endregion

        #region public System.Int32 SpamScore

        private System.Int32 _SpamScore;

        public System.Int32 SpamScore {
            get { return _SpamScore; }
            set { MarkDirty(); _SpamScore = value; }
        }

        #endregion

        #region public System.String IPAddress

        private System.String _IPAddress;

        public System.String IPAddress {
            get { return _IPAddress; }
            set { MarkDirty(); _IPAddress = value; }
        }

        #endregion

        #region public System.DateTime ModifiedOn

        private System.DateTime _ModifiedOn;

        public System.DateTime ModifiedOn {
            get { return _ModifiedOn; }
            set { MarkDirty(); _ModifiedOn = value; }
        }

        #endregion

        #region public System.String ModifiedBy

        private System.String _ModifiedBy;

        public System.String ModifiedBy {
            get { return _ModifiedBy; }
            set { MarkDirty(); _ModifiedBy = value; }
        }

        #endregion

        #region public System.String Email

        private System.String _Email;

        public System.String Email {
            get { return _Email; }
            set { MarkDirty(); _Email = value; }
        }

        #endregion

        #region public System.Boolean IsDeleted

        private System.Boolean _IsDeleted;

        public System.Boolean IsDeleted {
            get { return _IsDeleted; }
            set { MarkDirty(); _IsDeleted = value; }
        }

        #endregion

        #region public System.Guid UniqueId

        private System.Guid _UniqueId;

        public System.Guid UniqueId {
            get { return _UniqueId; }
            set { MarkDirty(); _UniqueId = value; }
        }

        #endregion

        #region public System.String UserName

        private System.String _UserName;

        public System.String UserName {
            get { return _UserName; }
            set { MarkDirty(); _UserName = value; }
        }

        #endregion

        #region public System.Boolean IsTrackback

        private System.Boolean _IsTrackback;

        public System.Boolean IsTrackback {
            get { return _IsTrackback; }
            set { MarkDirty(); _IsTrackback = value; }
        }

        #endregion

        /// <summary>
        /// The table object which represents Comment
        /// </summary>
        public static Table Table { get { return _Table; } }

        /// <summary>
        /// The columns which represent Comment
        /// </summary>
        public static class Columns {
            public static readonly Column Id = new Column("Id", DbType.Int32, typeof(System.Int32), "Id", false, true);
            public static readonly Column PostId = new Column("PostId", DbType.Int32, typeof(System.Int32), "PostId", false, false);
            public static readonly Column Body = new Column("Body", DbType.String, typeof(System.String), "Body", false, false);
            public static readonly Column CreatedBy = new Column("CreatedBy", DbType.String, typeof(System.String), "CreatedBy", true, false);
            public static readonly Column Published = new Column("Published", DbType.DateTime, typeof(System.DateTime), "Published", false, false);
            public static readonly Column Name = new Column("Name", DbType.String, typeof(System.String), "Name", false, false);
            public static readonly Column IsPublished = new Column("IsPublished", DbType.Boolean, typeof(System.Boolean), "IsPublished", false, false);
            public static readonly Column Version = new Column("Version", DbType.Int32, typeof(System.Int32), "Version", false, false);
            public static readonly Column WebSite = new Column("WebSite", DbType.String, typeof(System.String), "WebSite", true, false);
            public static readonly Column SpamScore = new Column("SpamScore", DbType.Int32, typeof(System.Int32), "SpamScore", false, false);
            public static readonly Column IPAddress = new Column("IPAddress", DbType.String, typeof(System.String), "IPAddress", true, false);
            public static readonly Column ModifiedOn = new Column("ModifiedOn", DbType.DateTime, typeof(System.DateTime), "ModifiedOn", false, false);
            public static readonly Column ModifiedBy = new Column("ModifiedBy", DbType.String, typeof(System.String), "ModifiedBy", true, false);
            public static readonly Column Email = new Column("Email", DbType.String, typeof(System.String), "Email", true, false);
            public static readonly Column IsDeleted = new Column("IsDeleted", DbType.Boolean, typeof(System.Boolean), "IsDeleted", false, false);
            public static readonly Column UniqueId = new Column("UniqueId", DbType.Guid, typeof(System.Guid), "UniqueId", false, false);
            public static readonly Column UserName = new Column("UserName", DbType.String, typeof(System.String), "UserName", true, false);
            public static readonly Column IsTrackback = new Column("IsTrackback", DbType.Boolean, typeof(System.Boolean), "IsTrackback", false, false);
        }

        public static int Delete(Column column, object value) {
            DataComment objectToDelete = FetchByColumn(column, value);
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
            DataComment objectToDelete = FetchByColumn(column, value);
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

            parameters.Add(new Parameter("PostId", null, DbType.Int32));

            Parameter pBody = new Parameter("Body", null, DbType.String);
            pBody.Length = 4000;
            parameters.Add(pBody);

            Parameter pCreatedBy = new Parameter("CreatedBy", null, DbType.String);
            pCreatedBy.Length = 128;
            parameters.Add(pCreatedBy);

            parameters.Add(new Parameter("Published", null, DbType.DateTime));

            Parameter pName = new Parameter("Name", null, DbType.String);
            pName.Length = 128;
            parameters.Add(pName);

            parameters.Add(new Parameter("IsPublished", null, DbType.Boolean));

            parameters.Add(new Parameter("Version", null, DbType.Int32));

            Parameter pWebSite = new Parameter("WebSite", null, DbType.String);
            pWebSite.Length = 512;
            parameters.Add(pWebSite);

            parameters.Add(new Parameter("SpamScore", null, DbType.Int32));

            Parameter pIPAddress = new Parameter("IPAddress", null, DbType.String);
            pIPAddress.Length = 64;
            parameters.Add(pIPAddress);

            parameters.Add(new Parameter("ModifiedOn", null, DbType.DateTime));

            Parameter pModifiedBy = new Parameter("ModifiedBy", null, DbType.String);
            pModifiedBy.Length = 128;
            parameters.Add(pModifiedBy);

            Parameter pEmail = new Parameter("Email", null, DbType.String);
            pEmail.Length = 128;
            parameters.Add(pEmail);

            parameters.Add(new Parameter("IsDeleted", null, DbType.Boolean));

            parameters.Add(new Parameter("UniqueId", null, DbType.Guid));

            Parameter pUserName = new Parameter("UserName", null, DbType.String);
            pUserName.Length = 128;
            parameters.Add(pUserName);

            parameters.Add(new Parameter("IsTrackback", null, DbType.Boolean));

            return parameters;
        }

        #endregion

        protected override List<Parameter> GetParameters() {
            List<Parameter> parameters = new List<Parameter>(Table.Columns.Count);

            if (!IsNew) {
                parameters.Add(new Parameter("Id", Id, DbType.Int32));
            }

            parameters.Add(new Parameter("PostId", PostId, DbType.Int32));

            Parameter pBody = new Parameter("Body", Body, DbType.String);
            pBody.Length = 4000;
            parameters.Add(pBody);

            Parameter pCreatedBy = new Parameter("CreatedBy", CreatedBy, DbType.String);
            pCreatedBy.Length = 128;
            parameters.Add(pCreatedBy);

            parameters.Add(new Parameter("Published", Published, DbType.DateTime));

            Parameter pName = new Parameter("Name", Name, DbType.String);
            pName.Length = 128;
            parameters.Add(pName);

            parameters.Add(new Parameter("IsPublished", IsPublished, DbType.Boolean));

            parameters.Add(new Parameter("Version", Version, DbType.Int32));

            Parameter pWebSite = new Parameter("WebSite", WebSite, DbType.String);
            pWebSite.Length = 512;
            parameters.Add(pWebSite);

            parameters.Add(new Parameter("SpamScore", SpamScore, DbType.Int32));

            Parameter pIPAddress = new Parameter("IPAddress", IPAddress, DbType.String);
            pIPAddress.Length = 64;
            parameters.Add(pIPAddress);

            parameters.Add(new Parameter("ModifiedOn", ModifiedOn, DbType.DateTime));

            Parameter pModifiedBy = new Parameter("ModifiedBy", ModifiedBy, DbType.String);
            pModifiedBy.Length = 128;
            parameters.Add(pModifiedBy);

            Parameter pEmail = new Parameter("Email", Email, DbType.String);
            pEmail.Length = 128;
            parameters.Add(pEmail);

            parameters.Add(new Parameter("IsDeleted", IsDeleted, DbType.Boolean));

            parameters.Add(new Parameter("UniqueId", UniqueId, DbType.Guid));

            Parameter pUserName = new Parameter("UserName", UserName, DbType.String);
            pUserName.Length = 128;
            parameters.Add(pUserName);

            parameters.Add(new Parameter("IsTrackback", IsTrackback, DbType.Boolean));

            return parameters;
        }


        private bool _dontSendEmail;

        public bool DontSendEmail
        {
            get { return _dontSendEmail; }
            set { _dontSendEmail = value; }
        }

        private bool _dontChangeUser;

        public bool DontChangeUser
        {
            get { return _dontChangeUser; }
            set { _dontChangeUser = value; }
        }

        #region PreUpdate
        protected override void BeforeValidate()
        {
            base.BeforeValidate();

            //By default we allow no markup
            if(IsNew)
            {
                UniqueId = Guid.NewGuid();
                Body = DataUtil.ConvertTextToHTML(Body);

                IGraffitiUser gu = GraffitiUsers.Current;

                if (gu != null) {
                    if (!DontChangeUser) {
                        Name = gu.ProperName;
                        WebSite = gu.WebSite;
                        Email = gu.Email;
                        IsPublished = true;
                        UserName = gu.Name;
                    }
                } else {
                    if (!string.IsNullOrEmpty(WebSite))
                        WebSite = HttpUtility.HtmlEncode(WebSite);

                    if (!string.IsNullOrEmpty(Email))
                        Email = HttpUtility.HtmlEncode(Email);

                    Name = HttpUtility.HtmlEncode(Name);
                    //SpamScore = CommentSettings.ScoreComment(this, new Post(PostId));
                    IsPublished = SpamScore < CommentSettings.Get().SpamScore;
                }
            }

        }
        #endregion 

        #region PostUpdate

        protected override void AfterRemove(bool isDestroy)
        {
            DataPost.UpdateCommentCount(this.PostId);

            ZCache.RemoveCache("Comments-" + PostId);
            ZCache.RemoveByPattern("Comments-Recent");
        }

        protected override void AfterCommit()
        {
            base.AfterCommit();

            DataPost.UpdateCommentCount(PostId);

            if (!DontSendEmail)
            {
                try
                {
                    EmailTemplateToolboxContext etc = new EmailTemplateToolboxContext();
                    etc.Put("comment", this);
                    EmailTemplate ef = new EmailTemplate();
                    ef.Context = etc;
                    ef.Subject = "New Comment: " + Post.Title;
                    ef.To = Post.User.Email;
                    ef.TemplateName = "comment.view";

                    Emailer.Send(ef);
                    Log.Info("Comment Sent", "Email sent to {0} ({1}) from the post \"{2}\" ({3}).", Post.User.ProperName, Post.User.Email, Post.Title, Post.Id);
                }
                catch (Exception ex)
                {
                    Log.Error("Email Failure", ex.Message);
                }
            }

            ZCache.RemoveCache("Comments-" + PostId);
            ZCache.RemoveByPattern("Comments-Recent");

        }

        #endregion

        public static void DeleteUnpublishedComments()
        {
            DeleteByColumn(Columns.IsPublished, false);
        }

        public static void DeleteDeletedComments()
        {
            DeleteByColumn(Columns.IsDeleted,true);
        }

        private static void DeleteByColumn(Column column,  bool state)
        {
            List<string> idsToDelete = new List<string>();
            List<int> postIdsChanged = new List<int>();

            Query q = CreateQuery();
            q.AndWhere(column,state);
            q.AndWhere(Columns.Published, DateTime.Now.AddDays(-1 * Int32.Parse(ConfigurationManager.AppSettings["Graffiti::Comments::DaysToDelete"] ?? "7")),Comparison.LessOrEquals);
            q.Top = "25";
            q.OrderByAsc(Columns.Published);
            DataCommentCollection cc = DataCommentCollection.FetchByQuery(q);
            foreach(DataComment c in cc)
            {
                idsToDelete.Add(c.Id.ToString());
                if (!postIdsChanged.Contains(c.PostId))
                    postIdsChanged.Add(c.PostId);
            }

            if(idsToDelete.Count > 0)
            {
                QueryCommand deleteCommand =
                    new QueryCommand("DELETE FROM graffiti_Comments where Id in (" + string.Join(",", idsToDelete.ToArray()) + ")");

                DataService.ExecuteNonQuery(deleteCommand);

                foreach(int pid in postIdsChanged)
                    DataPost.UpdateCommentCount(pid);

                Log.Info("Deleted Comments", idsToDelete.Count + " comment(s) were removed from the database since they were older than " + (ConfigurationManager.AppSettings["Graffiti::Comments::DaysToDelete"] ?? "7") + " days and marked as " + ((column.Name == "IsDeleted") ? " deleted" : " unpublished") );
            }
        }

        public static int GetPublishedCommentCount(string user)
        {
            int count = 0;

            QueryCommand cmd;

            cmd = new QueryCommand(
            @"select " + DataService.Provider.SqlCountFunction("a.PostId") + @" from graffiti_Comments a 
                        inner join graffiti_Posts b on a.PostId = b.Id
                        where b.IsPublished <> 0 and b.IsDeleted = 0
                        and a.IsPublished <> 0 and a.IsDeleted = 0 and b.Status = 1");

            if (!String.IsNullOrEmpty(user))
            {
                cmd.Sql += " and b.CreatedBy = " + DataService.Provider.SqlVariable("CreatedBy");
				cmd.Parameters.Add(DataPost.FindParameter("CreatedBy")).Value = user;
            }

            try
            {
                count = Convert.ToInt32(DataService.ExecuteScalar(cmd));
            }
            catch (Exception) { }

            return count;
        }


        #region Properties

        public IUser User {
            get {
                if (UserName != null)
                    return GraffitiUsers.GetUser(UserName);
                else
                    return new AnonymousUser(Name, WebSite);
            }
        }

        public DataPost Post
        {
            get
            {
                return DataPost.GetPost(PostId);
            }
        }

        /// <summary>
        /// Enables quick access to any comment.
        /// </summary>
        public string Url
        {
            get
            {
                return Post.Url + "#comment-" + Id;
            }
        }

        public string Title
        {
            get
            {
                return "RE: " + Post.Title;
            }
        }

        #endregion

        #region Comment Body Helpers

        //public static string ConvertTextToParagraph(string text)
        //{

        //    text = text.Replace("\r\n", "\n").Replace("\r", "\n");
        //    text += "\n\n";

        //    text = text.Replace("\n\n", "\n");

        //    string[] lines = text.Split('\n');

        //    StringBuilder paragraphs = new StringBuilder();

        //    foreach (string line in lines)
        //    {
        //        if (line != null && line.Trim().Length > 0)
        //            paragraphs.AppendFormat("<p>{0}</p>\n", line);
        //    }

        //    return paragraphs.ToString();
        //}


        //private static string FormatLinks(string text)
        //{
        //    if (string.IsNullOrEmpty(text))
        //        return text;



        //    //Find any links
        //    string pattern = @"(\s|^)(http|ftp|https):\/\/[\w]+(.[\w]+)([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])(\s|$)";
        //    MatchCollection matchs;
        //    StringCollection uniqueMatches = new StringCollection();

        //    matchs = Regex.Matches(text, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        //    foreach (Match m in matchs)
        //    {
        //        if (!uniqueMatches.Contains(m.ToString()))
        //        {
        //            string link = m.ToString().Trim();
        //            if (link.Length > 30)
        //            {
        //                try
        //                {
        //                    Uri u = new Uri(link);
        //                    string absolutePath = u.AbsolutePath.EndsWith("/")
        //                                    ? u.AbsolutePath.Substring(0, u.AbsolutePath.Length - 1)
        //                                    : u.AbsolutePath;

        //                    int slashIndex = absolutePath.LastIndexOf("/");
        //                    if (slashIndex > -1)
        //                        absolutePath = "/..." + absolutePath.Substring(slashIndex);

        //                    if (absolutePath.Length > 20)
        //                        absolutePath = absolutePath.Substring(0, 20);

        //                    link = u.Host + absolutePath;
        //                }
        //                catch
        //                {
        //                }
        //            }
        //            text = text.Replace(m.ToString(), " <a target=\"_blank\" href=\"" + m.ToString().Trim() + "\">" + link + "</a> ");
        //            uniqueMatches.Add(m.ToString());
        //        }
        //    }

        //    return text;
        //}


        #endregion 
    }
}