using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FeedApi.Models
{
    public class MemoryRepository : IRepository
    {
        private List<FeedCollection> items;

        public IEnumerable<FeedCollection> FeedCollections => items;

        public int CreateFeedCollection(FeedCollection item)
        {
            if (string.IsNullOrEmpty(item.Title))
                throw new ArgumentException("Title is null or empty!");
            if (items.Any(i => i.Title == item.Title))
                throw new DuplicateWaitObjectException($"This user has already exists Feed Collection with {item.Title} title!");

            item.Id = items.Max(i => i.Id) + 1;
            items.Add(item);

            return item.Id;
        }

        public IEnumerable<FeedCollection> GetUserFeedCollections(int userId)
        {
            return items.Where(i => i.UserId == userId);
        }

        public IEnumerable<FeedCollection> GetUserFeedCollection(int userId, int id)
        {
            return items.Where(i => i.UserId == userId && i.Id == id);
        }

        private FeedType GetFeedType(string url)
        {
            XDocument doc = XDocument.Load(url);
            foreach (var element in doc.Elements())
            {
                if (element.Name.LocalName == "rss")
                    return FeedType.RSS;

                if (element.Name.LocalName == "feed")
                    return FeedType.Atom;
            }

            throw new FormatException($"Invalid type of feed: {url}!");
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
                Id = feedCollection.Feeds.Max(i => i.Id) + 1,
                Url = url
            };
            feedCollection.Feeds.Add(feed);

            return feed.Id;
        }
    }
}
