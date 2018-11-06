using Newtonsoft.Json;
using System.Collections.Generic;

namespace FeedApi.Models
{
    public class Feed
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Url { get; set; }

        public List<FeedSource> Items { get; set; }
    }
}
