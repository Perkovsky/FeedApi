using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedApi.Models
{
    public class FeedCollection
    {
        [JsonIgnore]
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Title { get; set; }

        [JsonIgnore]
        public List<Feed> Feeds { get; set; }
    }
}
