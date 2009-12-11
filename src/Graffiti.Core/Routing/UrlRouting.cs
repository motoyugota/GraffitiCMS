using System;
using System.Web.Routing;

namespace Graffiti.Core
{
    public static class UrlRouting
    {
        public static void Initialize()
        {
            RegisterRoutes(RouteTable.Routes);
        }

        static void RegisterRoutes(RouteCollection routes)
        {
            routes.Add(new Route("{resource}.axd/{*pathInfo}", new StopRoutingHandler()));
            routes.Add(new Route("{resource}.ico/{*pathInfo}", new StopRoutingHandler()));
            routes.Add("TagPage", new Route("tags/{TagName}/", new TagHandler()));

            

            routes.Add("SiteFeed", new Route("feed/", new RssHandler()));
            routes.Add("Category1Feed", new Route("{CategoryOne}/feed/", new RssHandler()));
            routes.Add("Category2Feed", new Route("{CategoryOne}/{CategoryTwo}/feed/", new RssHandler()));

            routes.Add("Param1", new Route("{Param1}", new CategoryAndPostHandler()));
            routes.Add("Param2", new Route("{Param1}/{Param2}/", new CategoryAndPostHandler()));
            routes.Add("Param3", new Route("{Param1}/{Param2}/{Param3}/", new CategoryAndPostHandler()));
        }


    }
 }