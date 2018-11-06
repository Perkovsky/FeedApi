using System.Collections.Generic;

namespace FeedApi.Models
{
    public interface IRepository
    {
        IEnumerable<FeedCollection> FeedCollections { get; }

        int CreateFeedCollection(FeedCollection item);

        IEnumerable<FeedCollection> GetUserFeedCollections(int userId);

        IEnumerable<FeedCollection> GetUserFeedCollection(int userId, int id);

        int AddFeed(int userId, int feedCollectionId, string url);
    }
}
