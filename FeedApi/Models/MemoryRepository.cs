using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FeedApi.Models
{
    public class MemoryRepository : IRepository
    {
        private readonly IMemoryCache cache;
        private List<FeedCollection> items = new List<FeedCollection>();

        public bool IsEmpty => items.Count == 0;

        public MemoryRepository(IMemoryCache cache) => this.cache = cache;

        public IEnumerable<FeedCollection> GetUserFeedCollections(int userId)
        {
            return items.Where(i => i.UserId == userId);
        }

        private FeedSource GetFeedResponse(string url)
        {
            XDocument doc = XDocument.Load(url);
            foreach (var element in doc.Elements())
            {
                if (element.Name.LocalName == "rss")
                    return new RssFeedSource();

                if (element.Name.LocalName == "feed")
                    return new AtomFeedSource();
            }

            throw new FormatException($"Invalid type of feed: {url}!");
        }

        public IEnumerable<Feed> GetUserFeeds(int userId, int feedCollectionId)
        {
            var feedCollection = items.FirstOrDefault(i => i.Id == feedCollectionId);
            if (feedCollection == null)
                throw new ArgumentException($"Feed Collection with ID={feedCollectionId} not found!");

            var result = new List<Feed>();
            foreach (var feed in feedCollection.Feeds)
            {
                feed.Items = GetFeedResponse(feed.Url).GetFeeds(feed.Url).ToList();
                result.Add(feed);
            }

            return result;
        }

        public int CreateFeedCollection(FeedCollection item)
        {
            if (string.IsNullOrEmpty(item.Title))
                throw new ArgumentException("Title is null or empty!");
            if (items.Any(i => i.Title == item.Title))
                throw new DuplicateWaitObjectException($"This user has already exists Feed Collection with {item.Title} title!");

            item.Id = (items.Count == 0) ? 1 : items.Max(i => i.Id) + 1;
            items.Add(item);

            return item.Id;
        }

        public int AddFeed(int userId, int feedCollectionId, string url)
        {
            var feedCollection = items.FirstOrDefault(i => i.UserId == userId && i.Id == feedCollectionId);
            if (feedCollection == null)
                throw new ArgumentException($"Feed Collection with ID={feedCollectionId} not found!");

            if (feedCollection.Feeds.Any(i => i.Url == url))
                throw new DuplicateWaitObjectException("This feed is already exists!");

            var feed = new Feed
            {
                Id = (feedCollection.Feeds.Count == 0) ? 1 : feedCollection.Feeds.Max(i => i.Id) + 1,
                Url = url
            };
            feedCollection.Feeds.Add(feed);

            return feed.Id;
        }
    }
}
