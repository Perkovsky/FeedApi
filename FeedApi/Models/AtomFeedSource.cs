using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FeedApi.Models
{
    public class AtomFeedSource : FeedSource
    {
        public override IEnumerable<FeedSource> GetFeeds(string url)
        {
            XDocument doc = XDocument.Load(url);
            return doc.Root.Elements().Where(i => i.Name.LocalName == "entry")
                .Select(f => new RssFeedSource
                {
                    Content = f.Elements().FirstOrDefault(i => i.Name.LocalName == "summary")?.Value,
                    Link = f.Elements().FirstOrDefault(i => i.Name.LocalName == "link")?.Attribute("href")?.Value,
                    Date = GetDate(f.Elements().FirstOrDefault(i => i.Name.LocalName == "published")?.Value),
                    Title = f.Elements().FirstOrDefault(i => i.Name.LocalName == "title")?.Value
                });
        }
    }
}
