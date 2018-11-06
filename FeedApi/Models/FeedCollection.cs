using Newtonsoft.Json;
using System.Collections.Generic;

namespace FeedApi.Models
{
    public class FeedCollection
    {
        [JsonIgnore]
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Title { get; set; }

        [JsonIgnore]
        public List<Feed> Feeds { get; set; } = new List<Feed>();
    }
}
