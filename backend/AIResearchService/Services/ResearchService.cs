using AIResearchService.Common;
using AIResearchService.Interfaces;

namespace AIResearchService.Services
{
    public class ResearchService(IGoogleSearch google) : IResearchService
    {
        public async Task Research(string topic, TimeFrame timeFrame)
        {
            await google.Fetch(topic, Site.YouTube, timeFrame);
            await google.Fetch(topic, Site.Reddit, timeFrame);
        }
    }
}