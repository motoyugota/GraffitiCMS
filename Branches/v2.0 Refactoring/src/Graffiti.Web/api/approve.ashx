<%@ WebHandler Language="C#" Class="Graffiti.Web.Approve" %>

using System;
using System.Text;
using System.Web;
using System.Xml;
using Graffiti.Core;
using DataBuddy;
using Graffiti.Core.Services;
using System.Linq;

namespace Graffiti.Web
{
    public class Approve : IHttpHandler 
	{

		private IRolePermissionService _rolePermissionService = ServiceLocator.Get<IRolePermissionService>();
	    private IPostService _postService = ServiceLocator.Get<IPostService>();
	    private IVersionStoreService _versionStoreService = ServiceLocator.Get<IVersionStoreService>();
			
		public void ProcessRequest (HttpContext context) 
		{
            string u = context.Request.QueryString["u"];
            string key = context.Request.QueryString["key"];
            string p = context.Request.QueryString["id"];
            string v = context.Request.QueryString["v"];

            if (u != null || key != null || p != null || v != null)
            {
                IGraffitiUser user = GraffitiUsers.GetUser(u);
                var the_Post = _postService.FetchPost(p);

                Permission perm = _rolePermissionService.GetPermissions(the_Post.CategoryId, user);
                if (user != null && (GraffitiUsers.IsAdmin(user) || perm.Publish))
                {
                    if (user.UniqueId == new Guid(key))
                    {
                        if (!the_Post.IsNew)
                        {
                            int version = Int32.Parse(v);
                            if (the_Post.Version != version)
                            {
								var vs = _versionStoreService.FetchVersionStoreByPostId(the_Post.Id,version)
									.Where(x => x.Type == "post/xml")
									.FirstOrDefault();

                                if (vs != null)
                                    the_Post = Post.FromXML(vs.Data);
								else
									the_Post = null;
                            }

                        }
                        else
                        {
                            context.Response.Redirect("~/");
                        }

                        if (the_Post != null)
                        {
                            the_Post.PostStatus = PostStatus.Publish;
                            PostRevisionManager.CommitPost(the_Post, user, SiteSettings.Get().FeaturedId == the_Post.Id, the_Post.Category.FeaturedId == the_Post.Id);

                            context.Response.Redirect(the_Post.VirtualUrl);
                        }
                    }
                }
            }

            context.Response.Redirect("~/");
	        
		}
	 
		public bool IsReusable {
			get {
				return false;
			}
		}

	}
}