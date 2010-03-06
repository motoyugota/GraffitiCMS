using System;
using System.Web;
using System.Web.Routing;
using Graffiti.Core.Services;

namespace Graffiti.Core
{
	public class RssHandler : IRouteHandler
	{
        private ICategoryService _categoryService;

        public RssHandler(ICategoryService categoryService) 
        {
            _categoryService = categoryService;
        }

        public RssHandler()
        {
            _categoryService = ServiceLocator.Get<ICategoryService>();
        }

		public IHttpHandler GetHttpHandler(RequestContext requestContext)
		{
			string categoryOneName = requestContext.RouteData.Values["categoryone"] != null ? requestContext.RouteData.Values["categoryone"].ToString() : null;
			string categoryTwoName = requestContext.RouteData.Values["categorytwo"] != null ? requestContext.RouteData.Values["categorytwo"].ToString() : null;
			string tagName = requestContext.RouteData.Values["tagname"] != null ? requestContext.RouteData.Values["tagname"].ToString() : null;


			if (!String.IsNullOrEmpty(categoryTwoName))
			{
				return GetFeed(categoryTwoName);
			}

			if (!String.IsNullOrEmpty(categoryOneName))
			{
				return GetFeed(categoryOneName);
			}

			if (!string.IsNullOrEmpty(tagName))
			{
				return GetTagFeed(tagName);
			}

			return GetFeed(null);
		}

		public RSS GetTagFeed(string tagName)
		{
			var feed = new RSS();
			if (!string.IsNullOrEmpty(tagName))
			{
				feed.TagName = tagName;
			}
			return feed;
		}

		public RSS GetFeed(string categoryName)
		{
			var feed = new RSS();

			if (!String.IsNullOrEmpty(categoryName))
			{
				var category = _categoryService.FetchCachedCategoryByLinkName(categoryName, false);

				feed.CategoryID = category.Id;
				feed.CategoryName = category.LinkName;
			}

			return feed;
		}

	}
}