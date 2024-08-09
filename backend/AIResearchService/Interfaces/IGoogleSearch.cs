using AIResearchService.Common;

namespace AIResearchService.Interfaces;

public interface IGoogleSearch
{
    Task Fetch(string query, Site site = Site.None, TimeFrame time = TimeFrame.PastMonth, int numResults = 5);
}
