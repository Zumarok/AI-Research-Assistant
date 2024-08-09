using AIResearchService.Common;
using AIResearchService.Interfaces;

namespace AIResearchService.Repositories;

public class SearchRepository : ISearchRepository
{
    public SearchResultList SearchResults { get; } = new();
}
