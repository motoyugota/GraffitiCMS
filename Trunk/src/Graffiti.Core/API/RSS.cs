using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Xml;
using DataBuddy;

namespace Graffiti.Core
{
    public class RSS : Page
    {
        public int CategoryID = -1;
        public string CategoryName = null;
        protected DateTime lastModified = DateTime.Now;
        protected Macros macros = new Macros();

        protected override void OnLoad(EventArgs e)
        {
            Initialize();

            SiteSettings settings = SiteSettings.Get();

            Category category = null;
            if (CategoryID > -1)
                category = new CategoryController().GetCachedCategory(CategoryID, false);

            if(category == null)
            {
                if (!string.IsNullOrEmpty(settings.ExternalFeedUrl) && Request.UserAgent.IndexOf("FeedBurner",StringComparison.InvariantCultureIgnoreCase) == -1)
                {
                    Context.Response.RedirectLocation = settings.ExternalFeedUrl;
                    Context.Response.StatusCode = 301;
                    Context.Response.End();
                }
            }
            else if (!string.IsNullOrEmpty(category.FeedUrlOverride) && Request.UserAgent.IndexOf("FeedBurner", StringComparison.InvariantCultureIgnoreCase) == -1)
            {
                Context.Response.RedirectLocation = category.FeedUrlOverride;
                Context.Response.StatusCode = 301;
                Context.Response.End();
            }
            else if(CategoryName != null && !Util.AreEqualIgnoreCase(CategoryName,category.LinkName))
            {
                Context.Response.RedirectLocation = new Uri(Context.Request.Url, category.Url).ToString();
                Context.Response.StatusCode = 301;
                Context.Response.End();
            }

            string cacheKey = CategoryID > -1
                      ? "Posts-Index-" + Util.PageSize + "-" + CategoryID.ToString()
                      : string.Format("Posts-Categories-P:{0}-C:{1}-T:{2}-PS:{3}", 1, CategoryID, SortOrderType.Descending, Util.PageSize);

            PostCollection pc = ZCache.Get<PostCollection>(cacheKey);
            
            if(pc == null)
            {
                Query q = PostCollection.DefaultQuery();
                q.Top = Util.PageSize.ToString();
                if (CategoryID > 0)
                    q.AndWhere(Post.Columns.CategoryId, CategoryID);

                pc = new PostCollection();
                pc.LoadAndCloseReader(q.ExecuteReader());

                PostCollection permissionsFiltered = new PostCollection();

                permissionsFiltered.AddRange(pc);

                foreach (Post p in pc)
                {
                    if (!RolePermissionManager.GetPermissions(p.CategoryId, GraffitiUsers.Current).Read)
                        permissionsFiltered.Remove(p);
                }

                ZCache.InsertCache(cacheKey, permissionsFiltered, 90);
            }

            if (pc.Count > 0)
            {
                string lastMod = Context.Request.Headers["If-Modified-Since"];
                if (lastMod != null)
                {
                    if (lastMod == pc[0].Published.AddHours(-1 * settings.TimeZoneOffSet).ToUniversalTime().ToString("r"))
                    {
                        Context.Response.StatusCode = 304;
                        Context.Response.Status = "304 Not Modified";
                        Context.Response.End();
                    }
                }
                lastModified = pc[0].Published.AddHours(-1 * settings.TimeZoneOffSet);
                Context.Response.Clear();
                Context.Response.Cache.SetCacheability(HttpCacheability.Public);
                Context.Response.Cache.SetLastModified(lastModified);
                Context.Response.Cache.SetETag(lastModified.ToString());
            }

            StringWriter sw = new StringWriter();
            sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
            XmlTextWriter writer = new XmlTextWriter(sw);

            writer.WriteStartElement("rss");
            writer.WriteAttributeString("version", "2.0");
            writer.WriteAttributeString("xmlns:dc", "http://purl.org/dc/elements/1.1/");
            writer.WriteAttributeString("xmlns:slash", "http://purl.org/rss/1.0/modules/slash/");

            // Allow plugins to add additional xml namespaces
            Graffiti.Core.Events.Instance().ExecuteRssNamespace(writer);

            string baseUrl = SiteSettings.BaseUrl;

            writer.WriteStartElement("channel");
            WriteChannel(writer, category, settings);

            
            
            foreach (Post p in pc)
            {
                writer.WriteStartElement("item");
                WriteItem(writer, p, settings, baseUrl);

                // Allow plugins to add additional xml to the <item>
                Graffiti.Core.Events.Instance().ExecuteRssItem(writer, p);

                writer.WriteEndElement(); // End Item
            }

            writer.WriteEndElement(); // End Channel
            writer.WriteEndElement(); // End Document

            // save XML into response
            Context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            Context.Response.ContentType = "application/rss+xml";
            Context.Response.Write(sw.ToString());
        }

        protected void WriteChannel(XmlTextWriter writer, Category category, SiteSettings settings)
        {
            writer.WriteElementString("title", category == null ? settings.Title : string.Concat(settings.Title, ": ", category.Name));
            writer.WriteElementString("link", macros.FullUrl(category == null ? new Urls().Home : category.Url));
            writer.WriteElementString("description", category == null ? settings.TagLine : Util.Truncate(category.Body, 250));
            writer.WriteElementString("generator", SiteSettings.VersionDescription);
            writer.WriteElementString("lastBuildDate", lastModified.AddHours(-1 * settings.TimeZoneOffSet).ToUniversalTime().ToString("r"));
        }

        protected void WriteItem(XmlTextWriter writer, Post p, SiteSettings settings, string baseUrl)
        {
            string link = macros.FullUrl(p.Url);
            writer.WriteElementString("title", HttpUtility.HtmlDecode(p.Title));
            writer.WriteElementString("link", link);
            writer.WriteElementString("pubDate", p.Published.AddHours(-1 * settings.TimeZoneOffSet).ToUniversalTime().ToString("r"));

            writer.WriteStartElement("guid");
            writer.WriteAttributeString("isPermaLink", "true");
            writer.WriteString(link);
            writer.WriteEndElement();

            if (!string.IsNullOrEmpty(p.UserName) && !string.IsNullOrEmpty(p.User.ProperName))
                writer.WriteElementString("dc:creator", p.User.ProperName);

            if (p.EnableComments)
            {
                writer.WriteElementString("slash:comments", p.CommentCount.ToString());
            }

            writer.WriteStartElement("category");
            writer.WriteAttributeString("domain", macros.FullUrl(p.Category.Url));
            writer.WriteString(p.Category.Name);
            writer.WriteEndElement();

            writer.WriteElementString("description", Util.FullyQualifyRelativeUrls(p.RenderBody(PostRenderLocation.Feed), baseUrl));
        }


        protected virtual void Initialize()
        {
            
        }
    }
}