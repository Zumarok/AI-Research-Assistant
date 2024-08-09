using System.Text.Json.Serialization;

namespace AIResearchService.Common;

/// <summary>
/// Data class which holds a web search result.
/// </summary>
public class SearchResult
{
    private string? _content = null;

    public string Title { get; set; }
    public string Url { get; set; }
    public int Index { get; set; }

    [JsonIgnore]
    public Site Site { get; set; }
    [JsonIgnore]
    public string Content => _content ?? FetchContent();

    private string FetchContent()
    {
        switch (Site)
        {
            case Site.YouTube:
                _content = GetWebContent.YoutubeTranscript(Url);
                break;
            case Site.Reddit:
                
                var result = Task.Run(() => GetWebContent.RedditThread(Url)).Result;
                _content = $"""
                         This is content of the Reddit thread titled '{Title}' in JSON format fetched from the internet.
                         Do not discuss the data structure itself.
                         Please give a detailed summary of the contents for the user, then ask if they'd like more information about this article, or if they'd like to fetch a different one.
                            - Discuss the subreddit where the thread was posted.
                            - Highlight specific comments, mentioning the author, what the comment is in response to, the score, and the date it was posted.
                            - Provide an overall summary of the thread's discussion, focusing on the key points raised in the comments.
                            
                         ```
                         {result}
                         ```
                         """;
                break;
            case Site.Wikipedia:
                _content = "";
                break;
            case Site.News:
            case Site.None:
                _content = "";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return _content;
    }
}
