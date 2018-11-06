using Newtonsoft.Json;

namespace FeedApi.Models
{
    public class Feed
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Url { get; set; }
    }
}
