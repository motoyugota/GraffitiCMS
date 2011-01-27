using System;
using System.Collections.Generic;
using System.Security;
using Graffiti.Core.Services;

namespace Graffiti.Core
{
    public static class FeedManager
    {
        private static IObjectStoreService _objectStoreService = ServiceLocator.Get<IObjectStoreService>();

        public static Feed AddFeed(Feed feed)
        {
            if (feed.Name == null)
                throw new Exception("Must give the feed a name");

            if (feed.Id == Guid.Empty)
                feed.Id = Guid.NewGuid();

            if (feed.RequestInterval == 0)
                feed.RequestInterval = 60;

           if(feed.Document == null)
               feed.ReLoad();

            ObjectStore os = new ObjectStore();
            os.UniqueId = feed.Id;
            os.Name = "Feed: " + feed.Name;
            os.Data = ObjectManager.ConvertToString(feed);
            os.ContentType = "feed/xml";
            os.Type = typeof (Feed).FullName;
            os = _objectStoreService.SaveObjectStore(os);

            ZCache.RemoveCache("Feed-Objects");

            return feed;
        }

        public static void UpdateFeed(Feed feed)
        {
            UpdateFeed(feed,true);
        }

        private static void UpdateFeed(Feed feed, bool resetCache)
        {
            ObjectStore os = _objectStoreService.FetchByUniqueId(feed.Id);
            os.Data = ObjectManager.ConvertToString(feed);
            os.Version++;
            os = _objectStoreService.SaveObjectStore(os);

            if(resetCache)
                ZCache.RemoveCache("Feed-Objects");
        }

        public static void UpdateFeedData()
        {
            DateTime dt = DateTime.Now;
            foreach(Feed feed in GetFeeds().Values)
            {
                if(feed.LastRequested.AddMinutes(feed.RequestInterval) <= dt)
                {
                    feed.ReLoad();
                    UpdateFeed(feed, false);
                }
            }

            ZCache.RemoveCache("Feed-Objects");
        }

        public static Dictionary<Guid,Feed> GetFeeds()
        {
            Dictionary<Guid, Feed> feeds = ZCache.Get<Dictionary<Guid, Feed>>("Feed-Objects");
            if(feeds == null)
            {
                feeds = new Dictionary<Guid, Feed>();

                foreach (ObjectStore os in _objectStoreService.FetchByContentType("feed/xml"))
                {
                    Feed feed = ObjectManager.ConvertToObject<Feed>(os.Data);
                    feeds.Add(feed.Id,feed);
                }

                ZCache.InsertCache("Feed-Objects", feeds,300);
            }

            return feeds;
        }

        public static Feed GetFeed(Guid id)
        {
            Dictionary<Guid, Feed> feeds = GetFeeds();
            if (feeds.ContainsKey(id))
                return feeds[id];

            Log.Warn("Feed", "Feed with id {0} was requested but not found", id);

            return null;
        }

        public static Feed GetFeed(string url)
        {
            return GetFeed(url, false);
        }

        public static Feed GetFeed(string url, bool throwExceptionOnError)
        {
            Dictionary<Guid, Feed> feeds = GetFeeds();
            foreach (Feed feed in feeds.Values)
                if (Util.AreEqualIgnoreCase(feed.Url, url))
                    return feed;

            Log.Warn("Feed", "Feed with name: {0} was requested but not found. It was added to the queue.", url);

            Feed newFeed = new Feed();
            newFeed.Url = url;
            newFeed.Id = Guid.NewGuid();
            newFeed.RequestInterval =60;
            newFeed.ReLoad(throwExceptionOnError);
            if (newFeed.Document != null)
                newFeed.Name = "Feed: " + newFeed.Document.Channel.Title;
            else
                newFeed.Name = "Feed:" + url;

            AddFeed(newFeed);

            return newFeed;


        }

        public static bool RegisterFeed(string url)
        {
            try
            {
                Feed feed = GetFeed(url,true);
                return feed != null;
            }
            catch(SecurityException secEx)
            {
                throw new SecurityException("Sorry, your feed " + url + " could be not processed. It is likely your site is configured to run under medium trust and cannot make external requests. Please contact your server host or administrator.", secEx);
            }
            catch(Exception ex)
            {
                throw new Exception("Sorry, your feed " + url + " could not be processed.", ex);
            }
        }
    }
}
