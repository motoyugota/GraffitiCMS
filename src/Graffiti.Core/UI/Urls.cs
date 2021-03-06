using System;
using System.Web;

namespace Graffiti.Core
{
	/// <summary>
	///     Urls returns different paths to your website. ($urls)
	/// </summary>
	[Chalk("urls")]
	public class Urls
	{
		/// <summary>
		///     The path to the login directory
		/// </summary>
		public string Home
		{
			get { return VirtualPathUtility.ToAbsolute("~/"); }
		}

		/// <summary>
		///     The path to the admin login page
		/// </summary>
		public string Login
		{
			get { return VirtualPathUtility.ToAbsolute("~/login/"); }
		}

		/// <summary>
		///     The path to logout the current user
		/// </summary>
		public string Logout
		{
			get { return VirtualPathUtility.ToAbsolute("~/graffiti-admin/logout/"); }
		}

		/// <summary>
		///     The path to the admin dashboard
		/// </summary>
		public string Admin
		{
			get { return VirtualPathUtility.ToAbsolute("~/graffiti-admin/"); }
		}

		/// <summary>
		///     The path to write a new post
		/// </summary>
		public string Write
		{
			get { return VirtualPathUtility.ToAbsolute("~/graffiti-admin/posts/write/"); }
		}

		/// <summary>
		///     The path to the search results page
		/// </summary>
		public string Search
		{
			get { return VirtualPathUtility.ToAbsolute("~/search/"); }
		}

		/// <summary>
		///     The path to the ajax handler for the application
		/// </summary>
		public string Ajax
		{
			get { return VirtualPathUtility.ToAbsolute("~/ajax.ashx"); }
		}

		/// <summary>
		///     The path to the ajax handler for the admin application
		/// </summary>
		public string AdminAjax
		{
			get { return VirtualPathUtility.ToAbsolute("~/graffiti-admin/ajax.ashx"); }
		}

		/// <summary>
		///     The path to the default page of the integrated marketplace, showing a list of collections
		/// </summary>
		public string AdminMarketplaceHome
		{
			get { return VirtualPathUtility.ToAbsolute("~/graffiti-admin/marketplace/"); }
		}

		/// <summary>
		///     The path to the users RSS feed. This will return an external feed (feedburner) if setup.
		/// </summary>
		public string Rss
		{
			get { return SiteSettings.Get().ExternalFeedUrl ?? VirtualPathUtility.ToAbsolute("~/feed/"); }
		}

		/// <summary>
		///     The path to to the tags used
		/// </summary>
		public string Tags
		{
			get { return VirtualPathUtility.ToAbsolute("~/tags/"); }
		}

		/// <summary>
		///     Gets the path to edit an existing post
		/// </summary>
		/// <param name="postId"></param>
		/// <returns></returns>
		public string Edit(object postId)
		{
			return VirtualPathUtility.ToAbsolute("~/graffiti-admin/posts/write/") + "?id=" + postId;
		}

		public string AdminMarketplace(string catalogType)
		{
			return string.Format("{0}?catalog={1}", VirtualPathUtility.ToAbsolute("~/graffiti-admin/marketplace/catalog.aspx"),
			                     catalogType);
		}

		public string AdminMarketplaceItem(string catalogType, int itemID)
		{
			return string.Format("{0}?catalog={1}&item={2}",
			                     VirtualPathUtility.ToAbsolute("~/graffiti-admin/marketplace/CatalogItem.aspx"), catalogType,
			                     itemID);
		}

		/// <summary>
		///     Gets the path to the feed url for a specific category
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public string GetFeedUrl(Category c)
		{
			if (!String.IsNullOrEmpty(c.FeedUrlOverride))
				return c.FeedUrlOverride;
			else
				return c.Url + "feed";
		}
	}
}