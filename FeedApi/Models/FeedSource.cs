using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FeedApi.Models
{
    public abstract class FeedSource
    {
        public string Link { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }

        protected DateTime GetDate(string date)
        {
            return (DateTime.TryParse(date, out DateTime result)) ? result : DateTime.MinValue;
        }

        /// <summary>
        /// Default is RSS
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public virtual IEnumerable<FeedSource> GetFeeds(string url)
        {
            XDocument doc = XDocument.Load(url);
            return doc.Root.Descendants().First(i => i.Name.LocalName == "channel").Elements().Where(i => i.Name.LocalName == "item")
                .Select(f => new RssFeedSource
                {
                    Content = f.Elements().FirstOrDefault(i => i.Name.LocalName == "description")?.Value,
                    Link = f.Elements().FirstOrDefault(i => i.Name.LocalName == "link")?.Value,
                    Date = GetDate(f.Elements().FirstOrDefault(i => i.Name.LocalName == "pubDate")?.Value),
                    Title = f.Elements().FirstOrDefault(i => i.Name.LocalName == "title")?.Value
                });
        }
    }
}
