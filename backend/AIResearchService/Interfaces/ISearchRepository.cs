using AIResearchService.Common;

namespace AIResearchService.Interfaces;

public interface ISearchRepository
{
    public SearchResultList SearchResults { get; }
}
