using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.IO;
using System.Web;
using Graffiti.Core;
using Graffiti.Core.Services;

namespace Graffiti.Web
{

	public class graffiti_admin_ajax : IHttpHandler
	{
		private IRolePermissionService _rolePermissionService = ServiceLocator.Get<IRolePermissionService>();
		private ICommentService _commentService = ServiceLocator.Get<ICommentService>();
		private ICategoryService _categoryService = ServiceLocator.Get<ICategoryService>();
		private IPostService _postService = ServiceLocator.Get<IPostService>();

		public void ProcessRequest(HttpContext context)
		{
			if (context.Request.RequestType != "POST" || !context.Request.IsAuthenticated)
				return;

			IGraffitiUser user = GraffitiUsers.Current;
			if (user == null)
				return;

			if (!_rolePermissionService.CanViewControlPanel(user))
				return;

			context.Response.ContentType = "text/plain";


			switch (context.Request.QueryString["command"])
			{
				case "deleteComment":

					Comment c = _commentService.FetchComment(context.Request.Form["commentid"]);

					if (_rolePermissionService.GetPermissions(c.Post.CategoryId, GraffitiUsers.Current).Publish)
					{
						_commentService.DeleteComment(int.Parse(context.Request.Form["commentid"]));
						context.Response.Write("success");
					}

					break;

				case "deleteCommentWithStatus":

					Comment c1 = _commentService.FetchComment(context.Request.Form["commentid"]);

					if (_rolePermissionService.GetPermissions(c1.Post.CategoryId, GraffitiUsers.Current).Publish)
					{
						_commentService.DeleteComment(int.Parse(context.Request.Form["commentid"]));
						context.Response.Write("The comment was deleted. <a href=\"javascript:void(0);\" onclick=\"Comments.unDelete('" + new Urls().AdminAjax + "'," + context.Request.Form["commentid"] + "); return false;\">Undo?</a>");
					}
					break;

				case "unDelete":
					Comment c2 = _commentService.FetchComment(context.Request.Form["commentid"]);

					if (_rolePermissionService.GetPermissions(c2.Post.CategoryId, GraffitiUsers.Current).Publish)
					{
						Comment comment = _commentService.FetchComment(context.Request.Form["commentid"]);
						comment.IsDeleted = false;
						comment = _commentService.SaveComment(comment);
						context.Response.Write("The comment was un-deleted. You may need to refresh the page to see it");
					}
					break;

				case "approve":
					Comment c3 = _commentService.FetchComment(context.Request.Form["commentid"]);

					if (_rolePermissionService.GetPermissions(c3.Post.CategoryId, GraffitiUsers.Current).Publish)
					{
						Comment cmt = _commentService.FetchComment(context.Request.Form["commentid"]);
						cmt.IsDeleted = false;
						cmt.IsPublished = true;
						cmt = _commentService.SaveComment(cmt);
						context.Response.Write("The comment was un-deleted and/or approved. You may need to refresh the page to see it");
					}
					break;

				case "deletePost":
					try
					{
						Post postToDelete = _postService.FetchPost(context.Request.Form["postid"]);

						Permission perm = _rolePermissionService.GetPermissions(postToDelete.CategoryId, user);

						if (GraffitiUsers.IsAdmin(user) || perm.Publish)
						{
							postToDelete.IsDeleted = true;
							postToDelete = _postService.SavePost(postToDelete, user.Name, DateTime.Now);

							//Post.Delete(context.Request.Form["postid"]);
							//ZCache.RemoveByPattern("Posts-");
							//ZCache.RemoveCache("Post-" + context.Request.Form["postid"]);
							context.Response.Write("The post was deleted. <a href=\"javascript:void(0);\" onclick=\"Posts.unDeletePost('" + new Urls().AdminAjax + "'," + context.Request.Form["postid"] + "); return false;\">Undo?</a>");
						}
					}
					catch (Exception ex)
					{
						context.Response.Write(ex.Message);
					}
					break;

				case "unDeletePost":
					Post p = _postService.FetchPost(context.Request.Form["postid"]);
					p.IsDeleted = false;
					p = _postService.SavePost(p);
					//ZCache.RemoveByPattern("Posts-");
					//ZCache.RemoveCache("Post-" + context.Request.Form["postid"]);                
					//context.Response.Write("The post was un-deleted. You may need to fresh the page to see it");
					break;

				case "permanentDeletePost":
					Post tempPost = _postService.FetchPost(context.Request.Form["postid"]);
					_postService.DestroyDeletedPost(tempPost);

					string url = VirtualPathUtility.ToAbsolute("~/graffiti-admin/posts/") + "?status=-1&dstry=" +
									 context.Server.UrlEncode(tempPost.Title);

					context.Response.Write(url);
					break;

				case "createdWidget":
					string widgetID = context.Request.Form["id"];
					List<WidgetDescription> the_widgets = Widgets.GetAvailableWidgets();
					Widget widget = null;
					foreach (WidgetDescription wia in the_widgets)
					{
						if (wia.UniqueId == widgetID)
						{
							widget = Widgets.Create(wia.WidgetType);
							break;
						}
					}

					context.Response.Write(widget.Id.ToString());

					break;

				case "updateWidgetsOrder":

					try
					{
						string listID = context.Request.Form["id"];
						string list = "&" + context.Request.Form["list"];

						Widgets.ReOrder(listID, list);

						//StreamWriter sw = new StreamWriter(context.Server.MapPath("~/widgets.txt"), true);
						//sw.WriteLine(DateTime.Now);
						//sw.WriteLine();
						//sw.WriteLine(context.Request.Form["left"]);
						//sw.WriteLine(context.Request.Form["right"]);
						//sw.WriteLine(context.Request.Form["queue"]);
						//sw.WriteLine();
						//sw.Close();

						context.Response.Write("Saved!");
					}
					catch (Exception ex)
					{
						context.Response.Write(ex.Message);
					}
					break;

				case "deleteWidget":

					string deleteID = context.Request.Form["id"];
					Widgets.Delete(deleteID);
					context.Response.Write("The widget was removed!");

					break;

				case "createTextLink":
					DynamicNavigationItem di = new DynamicNavigationItem();
					di.NavigationType = DynamicNavigationType.Link;
					di.Text = context.Request.Form["text"];
					di.Href = context.Request.Form["href"];
					di.Id = Guid.NewGuid();
					NavigationSettings.Add(di);
					context.Response.Write(di.Id);

					break;

				case "deleteTextLink":
					Guid g = new Guid(context.Request.Form["id"]);
					NavigationSettings.Remove(g);
					context.Response.Write("Success");
					break;

				case "reOrderNavigation":
					try
					{
						string navItems = "&" + context.Request.Form["navItems"];
						NavigationSettings.ReOrder(navItems);
						context.Response.Write("Success");
					}
					catch (Exception ex)
					{
						context.Response.Write(ex.Message);
					}
					break;

				case "addNavigationItem":


					try
					{
						if (context.Request.Form["type"] == "Post")
						{
							Post navPost = _postService.FetchPostByUniqueId(new Guid(context.Request.Form["id"]));
							DynamicNavigationItem item = new DynamicNavigationItem();
							item.PostId = navPost.Id;
							item.Id = navPost.UniqueId;
							item.NavigationType = DynamicNavigationType.Post;
							NavigationSettings.Add(item);
							context.Response.Write("Success");
						}
						else if (context.Request.Form["type"] == "Category")
						{
							Category navCategory = _categoryService.FetchCategoryByUniqueId(new Guid(context.Request.Form["id"]));
							DynamicNavigationItem item = new DynamicNavigationItem();
							item.CategoryId = navCategory.Id;
							item.Id = navCategory.UniqueId;
							item.NavigationType = DynamicNavigationType.Category;
							NavigationSettings.Add(item);
							context.Response.Write("Success");
						}

					}
					catch (Exception exp)
					{
						context.Response.Write(exp.Message);
					}

					break;

				case "reOrderPosts":
					try
					{
						Dictionary<int, Post> posts = new Dictionary<int, Post>();
						foreach (Post post in _postService.FetchPostsByCategory(int.Parse(context.Request.QueryString["id"])))
						{
							posts[post.Id] = post;
						}

						string postOrder = context.Request.Form["posts"];
						int orderNumber = 1;
						foreach (string sId in postOrder.Split('&'))
						{
							Post post = null;
							posts.TryGetValue(int.Parse(sId), out post);
							if (post != null && post.SortOrder != orderNumber)
							{
								post.SortOrder = orderNumber;
								post = _postService.SavePost(post);
							}

							orderNumber++;
						}

						context.Response.Write("Success");
					}
					catch (Exception ex)
					{
						context.Response.Write(ex.Message);
					}
					break;

				case "reOrderHomePosts":
					try
					{
						Dictionary<int, Post> posts = new Dictionary<int, Post>();
						foreach (Post post in _postService.FetchPosts().Where(x => x.IsHome))
						{
							posts[post.Id] = post;
						}

						string postOrder = context.Request.Form["posts"];
						int orderNumber = 1;
						foreach (string sId in postOrder.Split('&'))
						{
							Post post = null;
							posts.TryGetValue(int.Parse(sId), out post);
							if (post != null && post.HomeSortOrder != orderNumber)
							{
								post.HomeSortOrder = orderNumber;
								post = _postService.SavePost(post);
							}

							orderNumber++;
						}

						context.Response.Write("Success");
					}
					catch (Exception ex)
					{
						context.Response.Write(ex.Message);
					}
					break;

				case "categoryForm":

					int selectedCategory = int.Parse(context.Request.QueryString["category"] ?? "-1");
					int postId = int.Parse(context.Request.QueryString["post"] ?? "-1");
					NameValueCollection nvcCustomFields;
					if (postId > 0)
						nvcCustomFields = _postService.FetchPost(postId).CustomFields();
					else
						nvcCustomFields = new NameValueCollection();

					CustomFormSettings cfs = CustomFormSettings.Get(selectedCategory);

					if (cfs.HasFields)
					{
						foreach (CustomField cf in cfs.Fields)
						{
							if (context.Request.Form[cf.Id.ToString()] != null)
								nvcCustomFields[cf.Name] = context.Request.Form[cf.Id.ToString()];
						}

						context.Response.Write(cfs.GetHtmlForm(nvcCustomFields, (postId < 1)));
					}
					else
						context.Response.Write("");

					break;

				case "toggleEventStatus":

					try
					{
						EventDetails ed = Events.GetEvent(context.Request.QueryString["t"]);
						ed.Enabled = !ed.Enabled;

						if (ed.Enabled)
							ed.Event.EventEnabled();
						else
							ed.Event.EventDisabled();

						Events.Save(ed);

						context.Response.Write(ed.Enabled ? "Enabled" : "Disabled");
					}
					catch (Exception ex)
					{
						context.Response.Write(ex.Message);
					}
					break;

				case "buildMainFeed":
					try
					{
						FileInfo mainFeedFileInfo = new FileInfo(HttpContext.Current.Server.MapPath("~/Feed/Default.aspx"));

						if (!mainFeedFileInfo.Directory.Exists)
							mainFeedFileInfo.Directory.Create();

						using (StreamWriter sw = new StreamWriter(mainFeedFileInfo.FullName, false))
						{
							sw.WriteLine("<%@ Page Language=\"C#\" Inherits=\"Graffiti.Core.RSS\" %>");
							sw.Close();
						}

						context.Response.Write("Success");
					}
					catch (Exception ex)
					{
						context.Response.Write(ex.Message);
						return;
					}

					break;

				case "buildCategoryPages":

					try
					{
						foreach (Category cat in _categoryService.FetchCachedCategories())
							_categoryService.WriteCategoryPages(cat);

						context.Response.Write("Success");
					}
					catch (Exception ex)
					{
						context.Response.Write(ex.Message);
						return;
					}

					break;

				case "buildPages":

					try
					{
						int pageIndex = Int32.Parse(context.Request.Form["p"]);
						int pageSize = 20;

						PostCollection pc = new PostCollection(_postService.FetchPosts().OrderByDescending(x => x.Id).ToList());

						if (pageIndex > 0 && pageSize > 0 && pc.Count > pageSize)
							pc = new PostCollection(pc.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList());

						if (pc.Count > 0)
						{

							foreach (Post postToWrite in pc)
							{
								_postService.WritePostPages(postToWrite);
								foreach (string tagName in Util.ConvertStringToList(postToWrite.TagList))
								{
									if (!string.IsNullOrEmpty(tagName))
										Tag.WritePage(tagName);
								}

							}

							context.Response.Write("Next");
						}
						else
						{
							context.Response.Write("Success");
						}

					}
					catch (Exception ex)
					{
						context.Response.Write(ex.Message);
						return;
					}



					break;

				case "importPosts":

					try
					{
						Post newPost = new Post();
						newPost.Title = HttpContext.Current.Server.HtmlDecode(context.Request.Form["subject"].ToString());

						string postName = HttpContext.Current.Server.HtmlDecode(context.Request.Form["name"].ToString());

						PostCollection pc = new PostCollection();

						if (!String.IsNullOrEmpty(postName))
						{
							pc = new PostCollection(_postService.FetchPosts().Where(x => x.Name == Util.CleanForUrl(postName)).ToList());
						}

						if (pc.Count > 0)
						{
							newPost.Name = "[RENAME ME - " + Guid.NewGuid().ToString().Substring(0, 7) + "]";
							newPost.Status = (int)PostStatus.Draft;
						}
						else if (String.IsNullOrEmpty(postName))
						{
							newPost.Name = "[RENAME ME - " + Guid.NewGuid().ToString().Substring(0, 7) + "]";
							newPost.Status = (int)PostStatus.Draft;
						}
						else
						{
							newPost.Name = postName;
							newPost.Status = (int)PostStatus.Publish;
						}

						if (String.IsNullOrEmpty(newPost.Title))
							newPost.Title = newPost.Name;



						newPost.PostBody = HttpContext.Current.Server.HtmlDecode(context.Request.Form["body"].ToString());
						newPost.CreatedOn = Convert.ToDateTime(context.Request.Form["createdon"]);
						newPost.CreatedBy = context.Request.Form["author"];
						newPost.ModifiedBy = context.Request.Form["author"];
						newPost.TagList = context.Request.Form["tags"];
						newPost.ContentType = "text/html";
						newPost.CategoryId = Convert.ToInt32(context.Request.Form["category"]);
						newPost.UserName = context.Request.Form["author"];
						newPost.EnableComments = true;
						newPost.Published = Convert.ToDateTime(context.Request.Form["createdon"]);
						newPost.IsPublished = Convert.ToBoolean(context.Request.Form["published"]);

						// this was causing too many posts to be in draft status.
						// updated text on migrator to flag users to just move their content/binary directory
						// into graffiti's root
						//if (context.Request.Form["method"] == "dasBlog")
						//{
						//    if (newPost.Body.ToLower().Contains("/content/binary/"))
						//        newPost.Status = (int)PostStatus.Draft;
						//}

						newPost = _postService.SavePost(newPost, GraffitiUsers.Current.Name);

						int postid = Convert.ToInt32(context.Request.Form["postid"]);

						IMigrateFrom temp = null;

						switch (context.Request.Form["method"])
						{
							case "CS2007Database":

								CS2007Database db = new CS2007Database();
								temp = (IMigrateFrom)db;

								break;
							case "Wordpress":

								Wordpress wp = new Wordpress();
								temp = (IMigrateFrom)wp;

								break;

							case "BlogML":

								BlogML bml = new BlogML();
								temp = (IMigrateFrom)bml;

								break;

							case "CS21Database":
								CS21Database csDb = new CS21Database();
								temp = (IMigrateFrom)csDb;

								break;

							case "dasBlog":
								dasBlog dasb = new dasBlog();
								temp = (IMigrateFrom)dasb;

								break;
						}

						List<MigratorComment> comments = temp.GetComments(postid);

						foreach (MigratorComment cmnt in comments)
						{
							Comment ct = new Comment();
							ct.PostId = newPost.Id;
							ct.Body = cmnt.Body;
							ct.Published = cmnt.PublishedOn;
							ct.IPAddress = cmnt.IPAddress;
							ct.WebSite = cmnt.WebSite;
							ct.Email = string.IsNullOrEmpty(cmnt.Email) ? "" : cmnt.Email;
							ct.Name = string.IsNullOrEmpty(cmnt.UserName) ? "" : cmnt.UserName;
							ct.IsPublished = cmnt.IsPublished;
							ct.IsTrackback = cmnt.IsTrackback;
							ct.SpamScore = cmnt.SpamScore;
							ct.DontSendEmail = true;
							ct.DontChangeUser = true;

							ct = _commentService.SaveComment(ct);

							Comment ctemp = _commentService.FetchComment(ct.Id);
							ctemp.DontSendEmail = true;
							ctemp.DontChangeUser = true;
							ctemp.Body = HttpContext.Current.Server.HtmlDecode(ctemp.Body);
							ctemp = _commentService.SaveComment(ctemp);
						}

						if (newPost.Status == (int)PostStatus.Publish)
							context.Response.Write("Success" + context.Request.Form["panel"]);
						else
							context.Response.Write("Warning" + context.Request.Form["panel"]);
					}
					catch (Exception ex)
					{

						context.Response.Write(context.Request.Form["panel"] + ":" + ex.Message);
					}

					break;

				case "saveHomeSortStatus":

					SiteSettings siteSettings = SiteSettings.Get();
					siteSettings.UseCustomHomeList = bool.Parse(context.Request.Form["ic"]);
					siteSettings.Save();
					context.Response.Write("Success");

					break;

				case "checkCategoryPermission":

					try
					{
						int catID = Int32.Parse(context.Request.QueryString["category"]);
						string permissionName = context.Request.QueryString["permission"];
						Permission perm = _rolePermissionService.GetPermissions(catID, user);

						bool permissionResult = false;
						switch (permissionName)
						{
							case "Publish":
								permissionResult = perm.Publish;
								break;
							case "Read":
								permissionResult = perm.Read;
								break;
							case "Edit":
								permissionResult = perm.Edit;
								break;
						}

						context.Response.Write(permissionResult.ToString().ToLower());
					}
					catch (Exception ex)
					{
						context.Response.Write(ex.Message);
					}
					break;

			}

		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}

	// end namespace
}