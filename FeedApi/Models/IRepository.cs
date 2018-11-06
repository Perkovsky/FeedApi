using System.Collections.Generic;

namespace FeedApi.Models
{
    public interface IRepository
    {
        bool IsEmpty { get; }

        int CreateFeedCollection(FeedCollection item);

        IEnumerable<FeedCollection> GetUserFeedCollections(int userId);

        IEnumerable<Feed> GetUserFeeds(int userId, int feedCollectionId);

        int AddFeed(int userId, int feedCollectionId, string url);
    }
}
