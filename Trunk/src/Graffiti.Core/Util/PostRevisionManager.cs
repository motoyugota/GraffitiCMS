using System;
using System.Collections.Generic;
using System.Web;
using Graffiti.Core.Services;

namespace Graffiti.Core
{
    public class PostRevisionManager
    {
        private static IPostService _postService = ServiceLocator.Get<IPostService>();
        private static ICategoryService _categoryService = ServiceLocator.Get<ICategoryService>();
        private static IRolePermissionService _rolePermissionService = ServiceLocator.Get<IRolePermissionService>();
        private static IVersionStoreService _versionStoreService = ServiceLocator.Get<IVersionStoreService>();

        public static void SendRequestedChangesMessage(Post p, IGraffitiUser user)
        {
            List<IGraffitiUser> users = new List<IGraffitiUser>();
            foreach (IGraffitiUser u in GraffitiUsers.GetUsers("*"))
            {
                if (GraffitiUsers.IsAdmin(u) || _rolePermissionService.GetPermissions(p.CategoryId, u).Publish)
                    users.Add(u);
            }

            Macros m = new Macros();

            EmailTemplateToolboxContext pttc = new EmailTemplateToolboxContext();
            pttc.Put("sitesettings", SiteSettings.Get());
            pttc.Put("post", p);
            pttc.Put("user", user);
            pttc.Put("macros", m);
            pttc.Put("home", m.FullUrl(new Urls().Home));
            pttc.Put("adminUrl",
                     m.FullUrl(VirtualPathUtility.ToAbsolute("~/graffiti-admin/posts/write/")) + "?id=" + p.Id + "&v=" +
                     p.Version);

            EmailTemplate template = new EmailTemplate();
            template.Context = pttc;
            template.To = p.User.Email;
            template.Subject = "Changes Requested: " + p.Title;
            template.TemplateName = "RequestChanges.view";

            try
            {
                Emailer.Send(template); 
                //Emailer.Send("RequestChanges.view", p.User.Email, "Changes Requested: " + p.Title, pttc);
                Log.Info("Post Changes Email", p.User.Email + " was sent an email requesting changes");
            }
            catch (Exception ex)
            {
                Log.Error("Email Requested Changes Error", ex.Message);
            }
            
        }

        public static void SendPReqiresApprovalMessage(Post p, IGraffitiUser user)
        {
            List<IGraffitiUser> users = new List<IGraffitiUser>();
            foreach(IGraffitiUser u in GraffitiUsers.GetUsers("*"))
            {
                if (GraffitiUsers.IsAdmin(u) || _rolePermissionService.GetPermissions(p.CategoryId, u).Publish)
                    users.Add(u);
            }
            
            Macros m = new Macros();
            EmailTemplateToolboxContext pttc = new EmailTemplateToolboxContext();
            pttc.Put("sitesettings", SiteSettings.Get());
            pttc.Put("post", p);
            pttc.Put("user", user);
            pttc.Put("macros", m);
            pttc.Put("home", m.FullUrl(new Urls().Home));
            pttc.Put("adminUrl",
                     m.FullUrl(VirtualPathUtility.ToAbsolute("~/graffiti-admin/posts/write/")) + "?id=" + p.Id + "&v=" +
                     p.Version);

            string adminApprovalUrl = m.FullUrl(VirtualPathUtility.ToAbsolute("~/api/approve.ashx")) + "?key={0}&u={1}&id={2}&v={3}";

            EmailTemplate template = new EmailTemplate();
            template.Context = pttc;
            template.Subject = "You have content to approve: " + p.Title;
            template.TemplateName = "QueuedPost.view";

            foreach (IGraffitiUser admin in users)
            {
                template.Context.Put("adminApprovalUrl", string.Format(adminApprovalUrl, admin.UniqueId, admin.Name, p.Id, p.Version));

                try
                {
                    template.To = admin.Email;
                    Emailer.Send(template);
                    
                    //Emailer.Send("QueuedPost.view", admin.Email, "You have content to approve: " + p.Title, pttc);
                }
                catch(Exception ex)
                {
                    Log.Error("Email Error", ex.Message);
                }
            }

            Log.Info("Post approval email", "{0} user(s) were sent an email to approve the post \"{1}\" (id: {2}).", users.Count,p.Title,p.Id);
        }

        public static int CommitPost(Post p, IGraffitiUser user, bool isFeaturedPost, bool isFeaturedCategory)
        {
            Permission perm = _rolePermissionService.GetPermissions(p.CategoryId, user);
            bool isMan = perm.Publish;
            bool isEdit = GraffitiUsers.IsAdmin(user);

            if (isMan || isEdit)
            {
                p.IsPublished = (p.PostStatus == PostStatus.Publish);
            }
            else
            {
                p.IsPublished = false;
            
                if(p.PostStatus != PostStatus.Draft && p.PostStatus != PostStatus.PendingApproval)
                {
                    p.PostStatus = PostStatus.Draft;
                }
            }

            p.ModifiedBy = user.Name;

            if(p.IsNew) //No VERSION WORK, just save it.
            {
                p.Version = 1;
                _postService.SavePost(p, user.Name,SiteSettings.CurrentUserTime);
            }
            else if(p.IsPublished) //Make a copy of the current post, then save this one.
            {
                Post old_Post = _postService.FetchPost(p.Id);

                //if(old_Post.PostStatus == PostStatus.Publish)
                VersionPost(old_Post);

                p.Version = GetNextVersionId(p.Id, p.Version);
                _postService.SavePost(p, user.Name);
            }
            else
            {
                p.Version = GetNextVersionId(p.Id, p.Version);
                VersionPost(p);
                _postService.UpdatePostStatus(p.Id,p.PostStatus);
            }

            ProcessFeaturedPosts(p, user, isFeaturedPost, isFeaturedCategory);

            if(p.PostStatus == PostStatus.PendingApproval)
                SendPReqiresApprovalMessage(p,user);
            else if(p.PostStatus == PostStatus.RequiresChanges)
                SendRequestedChangesMessage(p,user);

            return p.Id;
        }

        public static int GetNextVersionId(int postid, int currentVersion)
        {
            if (postid <= 0)
                return 1;
            else
            {
                return _versionStoreService.GetNextVersionId(postid, currentVersion);
            }
        }

        private static void ProcessFeaturedPosts(Post p, IGraffitiUser user, bool isFeaturedPost, bool isFeaturedCategory)
        {
            SiteSettings settings = SiteSettings.Get();
            if (p.IsPublished && isFeaturedPost)
            {
                settings.FeaturedId = p.Id;
                settings.Save();
            }
            else if (settings.FeaturedId == p.Id)
            {
                settings.FeaturedId = -1;
                settings.Save();
            }

            Category c = p.Category;
            if (p.IsPublished && isFeaturedCategory)
            {
                c.FeaturedId = p.Id;
                _categoryService.SaveCategory(c, user.Name);
            }
            else if (c.FeaturedId == p.Id)
            {
                c.FeaturedId = -1;
                _categoryService.SaveCategory(c, user.Name);
            }
        }

        public static void VersionPost(Post old_Post)
        {
            VersionStore vs = new VersionStore();
            vs.Type = "post/xml";
            vs.ItemId = old_Post.Id;
            vs.Name = "Post:" + old_Post.Id;
            vs.UniqueId = Guid.NewGuid();
            vs.Data = old_Post.ToXML();
            vs.Version = old_Post.Version;
            vs = _versionStoreService.SaveVersionStore(vs, GraffitiUsers.Current.Name);
        }
    }
}