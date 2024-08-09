using AIResearchService.Common;

namespace AIResearchService.Interfaces;

public interface IResearchService
{
    Task Research(string topic, TimeFrame timeFrame);
}
