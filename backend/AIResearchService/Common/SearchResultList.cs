using System.ComponentModel;
using System.Text.Json;

namespace AIResearchService.Common;

/// <summary>
/// List of web search results.
/// </summary>
public class SearchResultList : BindingList<SearchResult>
{
    private JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

    /// <summary>
    /// Get a list of all Titles in the SearchResult List.
    /// </summary>
    /// <returns>List of all titles as strings.</returns>
    public List<string> GetAllTitlesWithIndex() => Items.Select(sr => $"[{sr.Index}]: {sr.Title}").ToList();

    /// <summary>
    /// Get all titles as a JSON string.
    /// </summary>
    /// <returns>JSON Style string with all titles.</returns>
    public string GetAllTitlesJson() => JsonSerializer.Serialize(GetAllTitlesWithIndex(), _jsonOptions);

    /// <summary>
    /// Get the SearchResult by index.
    /// </summary>
    /// <param name="index">The index of the item to get</param>
    /// <returns>The first match as a SearchResult object.</returns>
    public SearchResult GetResultByIndex(int index) => Items[index];

    /// <summary>
    /// Get the content of an entry by its index.
    /// </summary>
    /// <param name="index">The index of the SearchResult</param>
    /// <returns>The content property of the SearchResult as a string.</returns>
    public string GetContentByIndex(int index) => Items[index].Content;

    /// <summary>
    /// The entire list as a Json string.
    /// </summary>
    public string ToJson() => JsonSerializer.Serialize(Items.ToList(), _jsonOptions);

}