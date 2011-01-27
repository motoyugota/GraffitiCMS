using System;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using Graffiti.Core.Services;

namespace Graffiti.Core
{
    [Serializable]
    public class Comment
    {
        //private IPostService _postService;

        //public Comment(IPostService postService)
        //{
        //    _postService = postService;
        //}

        //public Comment()
        //{
        //    _postService = ServiceLocator.Get<IPostService>();
        //}


        public int Id { get; set; }
        public int PostId { get; set; }
        public string Body { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Published { get; set; }
        public string Name { get; set; }
        public bool IsLoaded { get; set; }
        public bool IsPublished { get; set; }

        private bool _isNew = true;
        public bool IsNew {
            get { return _isNew; }
            set { _isNew = value; }
        }

        public int Version { get; set; }
        public string WebSite { get; set; }
        public int SpamScore { get; set; }
        public string IPAddress { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string Email { get; set; }
        public bool IsDeleted { get; set; }
        public Guid UniqueId { get; set; }
        public string UserName { get; set; }
        public bool IsTrackback { get; set; }
        public bool DontSendEmail { get; set; }
        public bool DontChangeUser { get; set; }
        public IUser User { get; set; }
        public Post Post { get; set; }

        /// <summary>
        /// Enables quick access to any comment.
        /// </summary>
        public string Url {
            get {
                return Post.Url + "#comment-" + Id;
            }
        }

        public string Title {
            get {
                return "RE: " + Post.Title;
            }
        }

        #region event stuff
        //protected override void BeforeValidate()
        //{
        //    //base.BeforeValidate();

        //    //By default we allow no markup
        //    if(IsNew)
        //    {
        //        UniqueId = Guid.NewGuid();
        //        Body = Util.ConvertTextToHTML(Body);
               
        //        IGraffitiUser gu = GraffitiUsers.Current;
                
        //        if (gu != null)
        //        {
        //            if (!DontChangeUser)
        //            {
        //                Name = gu.ProperName;
        //                WebSite = gu.WebSite;
        //                Email = gu.Email;
        //                IsPublished = true;
        //                UserName = gu.Name;
        //            }
        //        }
        //        else
        //        {
        //            if (!string.IsNullOrEmpty(WebSite))
        //                WebSite = HttpUtility.HtmlEncode(WebSite);

        //            if (!string.IsNullOrEmpty(Email))
        //                Email = HttpUtility.HtmlEncode(Email);
                    
        //            Name = HttpUtility.HtmlEncode(Name);
        //            SpamScore = CommentSettings.ScoreComment(this, new Post(PostId));
        //            IsPublished = SpamScore < CommentSettings.Get().SpamScore;
        //        }
        //    }

        //}


        //protected override void AfterRemove(bool isDestroy) 
        //{
        //    _postService.UpdateCommentCount(this.PostId);

        //    ZCache.RemoveCache("Comments-" + PostId);
        //    ZCache.RemoveByPattern("Comments-Recent");
        //}

        //protected override void AfterCommit() 
        //{
        //    base.AfterCommit();

        //    _postService.UpdateCommentCount(PostId);

        //    if (!DontSendEmail) {
        //        try {
        //            EmailTemplateToolboxContext etc = new EmailTemplateToolboxContext();
        //            etc.Put("comment", this);
        //            EmailTemplate ef = new EmailTemplate();
        //            ef.Context = etc;
        //            ef.Subject = "New Comment: " + Post.Title;
        //            ef.To = Post.User.Email;
        //            ef.TemplateName = "comment.view";

        //            Emailer.Send(ef);
        //            Log.Info("Comment Sent", "Email sent to {0} ({1}) from the post \"{2}\" ({3}).", Post.User.ProperName, Post.User.Email, Post.Title, Post.Id);
        //        } catch (Exception ex) {
        //            Log.Error("Email Failure", ex.Message);
        //        }
        //    }

        //    ZCache.RemoveCache("Comments-" + PostId);
        //    ZCache.RemoveByPattern("Comments-Recent");
        //}
        #endregion

        #region Comment Body Helpers

        public static string ConvertTextToParagraph(string text) 
        {

            text = text.Replace("\r\n", "\n").Replace("\r", "\n");
            text += "\n\n";

            text = text.Replace("\n\n", "\n");

            string[] lines = text.Split('\n');

            StringBuilder paragraphs = new StringBuilder();

            foreach (string line in lines) {
                if (line != null && line.Trim().Length > 0)
                    paragraphs.AppendFormat("<p>{0}</p>\n", line);
            }

            return paragraphs.ToString();
        }


        private static string FormatLinks(string text) {
            if (string.IsNullOrEmpty(text))
                return text;



            //Find any links
            string pattern = @"(\s|^)(http|ftp|https):\/\/[\w]+(.[\w]+)([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])(\s|$)";
            MatchCollection matchs;
            StringCollection uniqueMatches = new StringCollection();

            matchs = Regex.Matches(text, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            foreach (Match m in matchs) {
                if (!uniqueMatches.Contains(m.ToString())) {
                    string link = m.ToString().Trim();
                    if (link.Length > 30) {
                        try {
                            Uri u = new Uri(link);
                            string absolutePath = u.AbsolutePath.EndsWith("/")
                                            ? u.AbsolutePath.Substring(0, u.AbsolutePath.Length - 1)
                                            : u.AbsolutePath;

                            int slashIndex = absolutePath.LastIndexOf("/");
                            if (slashIndex > -1)
                                absolutePath = "/..." + absolutePath.Substring(slashIndex);

                            if (absolutePath.Length > 20)
                                absolutePath = absolutePath.Substring(0, 20);

                            link = u.Host + absolutePath;
                        } catch {
                        }
                    }
                    text = text.Replace(m.ToString(), " <a target=\"_blank\" href=\"" + m.ToString().Trim() + "\">" + link + "</a> ");
                    uniqueMatches.Add(m.ToString());
                }
            }

            return text;
        }


        #endregion 
    }
}