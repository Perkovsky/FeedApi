namespace FeedApi.Models.Requests
{
    public class AddFeedRequest
    {
        public int UserId { get; set; }
        public int FeedCollectionId { get; set; }
        public string Url { get; set; }
    }
}
