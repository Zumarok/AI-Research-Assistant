using YoutubeTranscriptApi;

namespace AIResearchService.Common;

/// <summary>
/// Functions to get reddit stuff for LLMs.
/// </summary>
public static class GetWebContent
{
    /// <summary>
    /// Get the transcript for a YouTube video from a URL.
    /// </summary>
    /// <param name="url">URL of the YouTube video</param>
    /// <returns>The transcript as a string.</returns>
    public static string YoutubeTranscript(string url)
    {
        if (!Uri.IsWellFormedUriString(url, UriKind.Absolute) || !url.Contains("watch?v="))
            return $"Invalid URL: {url}";

        var uri = new Uri(url).ToString();
        var startIndex = uri.IndexOf("?v=", StringComparison.OrdinalIgnoreCase);
        var videoId = uri.Substring(startIndex + 3);

        try
        {
            var transcriptParts = new YouTubeTranscriptApi().GetTranscript(videoId);
            var textParts = transcriptParts.Select(c => c.Text + " ");
            return string.Concat(textParts);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return ("An exception was thrown in the YouTubeTranscriptApi.");
        }
    }

    /// <summary>
    /// Gets an entire reddit thread from a valid url and returns it as a JSON string.
    /// </summary>
    /// <param name="url">URL to the reddit thread.</param>
    /// <returns>A JSON string containing the entire reddit thread.</returns>
    public static async Task<string> RedditThread(string url)
    {
        return await RedditConvert.GetThreadJson(url);
    }

}
