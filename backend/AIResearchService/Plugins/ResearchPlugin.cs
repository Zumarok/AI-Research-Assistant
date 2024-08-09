using System.ComponentModel;
using AIResearchService.Common;
using AIResearchService.Interfaces;
using Microsoft.SemanticKernel;

namespace AIResearchService.Plugins;

public class ResearchPlugin(IResearchService researchService, ISearchRepository repository)
{
    [KernelFunction("start_research")]
    [Description("Start an internet search on the topic. The resulting list of article titles are shown to the user and will be stored in the SearchRepository.")]
    public async Task<string> ResearchAsync([Description("The user specified topic to research")] string topic, [Description("Limit the search results to a time frame")]TimeFrame timeFrame = TimeFrame.All)
    { 
        repository.SearchResults.Clear();
        await researchService.Research(topic, timeFrame);
        return repository.SearchResults.GetAllTitlesJson();
    }
    
    [KernelFunction("fetch_content")]
    [Description("Get the contents of an internet article from the SearchRepository; returns a string that will include a full Reddit thread in JSON, or YouTube video transcript.")]
    public string GetContentFromRepository(int index)
    {
        return repository.SearchResults.GetContentByIndex(index);
    }

    //[KernelFunction("get_repository_titles")]
    //[Description("Get the titles of all contents in the SearchRepository; returns a string containing the contents; Returns a formatted string containing the title of the found content along with its index in the SearchRepository, this list only contains 'Indexes' and 'Titles'.")]
    //public string GetRepositoryTitles()
    //{
    //    return repository.SearchResults.GetAllTitlesJson();
    //}

    //[KernelFunction("clear_repository")]
    //[Description("Clear the SearchRepository")]
    //public void ClearRepository()
    //{
    //    repository.SearchResults.Clear();
    //}
}
