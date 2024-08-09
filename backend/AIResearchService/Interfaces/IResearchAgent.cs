using AIResearchService.Common;
using Microsoft.SemanticKernel;

namespace AIResearchService.Interfaces;

public interface IResearchAgent
{
    Task<ChatMessageContent> InitAsync();
    Task<ChatMessageContent> ChatWithUserAsync(string userInput);
    Task<ChatMessageContent> VerifyFetchedTitle(SearchResult search);
    Task<bool> ResetAsync();
    Task<ChatMessageContent> GetContent(string title);
    
}
