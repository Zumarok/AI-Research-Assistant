using AIResearchService.Interfaces;
using HtmlAgilityPack;

namespace AIResearchService.Common;

public class GoogleSearch(ISearchRepository repository) : IGoogleSearch
{
    private static Dictionary<Site, string> _siteUrls = new()
    {
        { Site.None, "" },
        { Site.YouTube, "site:https://www.youtube.com/" },
        { Site.Reddit, "site:https://www.reddit.com/" },
        { Site.Wikipedia, "site:https://en.wikipedia.org/" },
        { Site.News, "&tbm=nws" },
    };

    private static Dictionary<TimeFrame, string> _timeStrs = new()
    {
        { TimeFrame.All, "" },
        { TimeFrame.PastHour, "&tbs=qdr:h" },
        { TimeFrame.PastDay, "&tbs=qdr:m" },
        { TimeFrame.PastWeek, "&tbs=qdr:w" },
        { TimeFrame.PastMonth, "&tbs=qdr:m" },
        { TimeFrame.PastYear, "&tbs=qdr:y" },
    };


    /// <summary>
    /// Search Google and add the results to the Repository.
    /// </summary>
    /// <returns>A SearchResultList containing the found items.</returns>
    public async Task Fetch(string query, Site site, TimeFrame time, int numResults)
    {
        for (int i = 0; i < 1; i++)
        {
            var url = $"https://www.google.com/search?q={query}+{_siteUrls[site]}{_timeStrs[time]}&num={numResults}&start={i * 10}";
            var web = new HtmlWeb();
            web.UserAgent = "user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36";
            var doc = web.Load(url);
            var nodes = doc.DocumentNode.SelectNodes($"//div[@class='{(site == Site.YouTube ? "nhaZ2c" : "yuRUbf")}']");

            if (nodes == null) break;

            foreach (var node in nodes)
            {
                var result = new SearchResult
                {
                    Url = node.Descendants("a").FirstOrDefault()?.Attributes["href"].Value ?? "Invalid",
                    Title = node.Descendants("h3").FirstOrDefault()?.InnerText ?? "Invalid",
                    Site = site
                };
                
                repository.SearchResults.Add(result);
                result.Index = repository.SearchResults.IndexOf(result);
            }
        }
    }
}
