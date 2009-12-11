using System;
using System.Web;
using System.Web.Compilation;
using System.Web.Routing;

namespace Graffiti.Core
{
    public class RssHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            string categoryOneName = requestContext.RouteData.Values["categoryone"] != null ? requestContext.RouteData.Values["categoryone"].ToString() : null;
            string categoryTwoName = requestContext.RouteData.Values["categorytwo"] != null ? requestContext.RouteData.Values["categorytwo"].ToString() : null;
            
            if (!String.IsNullOrEmpty(categoryTwoName))
            {
                return GetFeed(categoryTwoName);
            }
            
            if(!String.IsNullOrEmpty(categoryOneName))
            {
                return GetFeed(categoryOneName);
            }

            return GetFeed(null);
        }

        public RSS GetFeed(string categoryName)
        {
            var feed = new RSS();

            if (!String.IsNullOrEmpty(categoryName))
            {
                var category = new CategoryController().GetCachedCategoryByLinkName(categoryName, false);

                feed.CategoryID = category.Id;
                feed.CategoryName = category.LinkName;
            }

            return feed;
        }
    }
}