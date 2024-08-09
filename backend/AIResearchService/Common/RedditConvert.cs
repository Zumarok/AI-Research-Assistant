using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace AIResearchService.Common;

/// <summary>
/// Class for reddit data handling. Designed to use for feeding data to an LLM.
/// </summary>
public static class RedditConvert
{
    /// <summary>
    /// Gets a json string representation of a reddit thread, from a URL.
    /// </summary>
    /// <param name="url">The full url to a reddit discussion thread.</param>
    /// <returns>The Json string representation of the reddit thread.</returns>
    public static async Task<string> GetThreadJson(string url)
    {
        if (!Regex.IsMatch(url, @"\/comments\/([a-z0-9]+)\/"))
            return "Invalid reddit thread URL format.";

        url += ".json";
        string json = "";

        try
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36");
            json = await client.GetStringAsync(url);

            var o = JsonConvert.DeserializeObject<List<Root>>(json);
            return JsonConvert.SerializeObject(o, Formatting.Indented);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while downloading JSON from {url}: {ex.Message}");
        }
        return $"An error occurred while downloading JSON from {url}";
    }

    #region Reddit Thread Objects

    public class Child
    {
        [JsonIgnore]
        public string kind { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        [JsonIgnore]
        public object after { get; set; }
        [JsonIgnore]
        public int? dist { get; set; }
        [JsonIgnore]
        public string modhash { get; set; }
        [JsonIgnore]
        public string geo_filter { get; set; }
        public List<Child> children { get; set; }
        [JsonIgnore]
        public object before { get; set; }
        [JsonIgnore]
        public object approved_at_utc { get; set; }
        [JsonIgnore]
        public string subreddit { get; set; }


        public string selftext { get; set; }
        [JsonIgnore]
        public List<object> user_reports { get; set; }
        [JsonIgnore]
        public bool saved { get; set; }
        [JsonIgnore]
        public object mod_reason_title { get; set; }
        [JsonIgnore]
        public int gilded { get; set; }
        [JsonIgnore]
        public bool clicked { get; set; }


        public string title { get; set; }
        [JsonIgnore]
        public List<object> link_flair_richtext { get; set; }


        public string subreddit_name_prefixed { get; set; }
        [JsonIgnore]
        public bool hidden { get; set; }
        [JsonIgnore]
        public int pwls { get; set; }
        [JsonIgnore]
        public string link_flair_css_class { get; set; }
        [JsonIgnore]
        public int downs { get; set; }
        [JsonIgnore]
        public object thumbnail_height { get; set; }
        [JsonIgnore]
        public object top_awarded_type { get; set; }
        [JsonIgnore]
        public string parent_whitelist_status { get; set; }
        [JsonIgnore]
        public bool hide_score { get; set; }
        [JsonIgnore]
        public string name { get; set; }
        [JsonIgnore]
        public bool quarantine { get; set; }
        [JsonIgnore]
        public string link_flair_text_color { get; set; }
        [JsonIgnore]
        public int upvote_ratio { get; set; }
        [JsonIgnore]
        public string author_flair_background_color { get; set; }
        [JsonIgnore]
        public string subreddit_type { get; set; }
        [JsonIgnore]
        public int ups { get; set; }
        [JsonIgnore]
        public int total_awards_received { get; set; }
        [JsonIgnore]
        public MediaEmbed media_embed { get; set; }
        [JsonIgnore]
        public object thumbnail_width { get; set; }
        [JsonIgnore]
        public object author_flair_template_id { get; set; }
        [JsonIgnore]
        public bool is_original_content { get; set; }
        [JsonIgnore]
        public string author_fullname { get; set; }
        [JsonIgnore]
        public object secure_media { get; set; }
        [JsonIgnore]
        public bool is_reddit_media_domain { get; set; }
        [JsonIgnore]
        public bool is_meta { get; set; }
        [JsonIgnore]
        public object category { get; set; }
        [JsonIgnore]
        public SecureMediaEmbed secure_media_embed { get; set; }
        [JsonIgnore]
        public string link_flair_text { get; set; }
        [JsonIgnore]
        public bool can_mod_post { get; set; }


        public int score { get; set; }
        [JsonIgnore]
        public object approved_by { get; set; }
        [JsonIgnore]
        public bool is_created_from_ads_ui { get; set; }
        [JsonIgnore]
        public bool author_premium { get; set; }
        [JsonIgnore]
        public string thumbnail { get; set; }
        [JsonIgnore]
        public object edited { get; set; }
        [JsonIgnore]
        public object author_flair_css_class { get; set; }
        [JsonIgnore]
        public List<object> author_flair_richtext { get; set; }
        [JsonIgnore]
        public Gildings gildings { get; set; }
        [JsonIgnore]
        public string post_hint { get; set; }
        [JsonIgnore]
        public object content_categories { get; set; }
        [JsonIgnore]
        public bool is_self { get; set; }
        [JsonIgnore]
        public object mod_note { get; set; }
        [JsonIgnore]
        public int created { get; set; }
        [JsonIgnore]
        public string link_flair_type { get; set; }
        [JsonIgnore]
        public int wls { get; set; }
        [JsonIgnore]
        public object removed_by_category { get; set; }
        [JsonIgnore]
        public object banned_by { get; set; }
        [JsonIgnore]
        public string author_flair_type { get; set; }
        [JsonIgnore]
        public string domain { get; set; }
        [JsonIgnore]
        public bool allow_live_comments { get; set; }
        [JsonIgnore]
        public string selftext_html { get; set; }
        [JsonIgnore]
        public object likes { get; set; }
        [JsonIgnore]
        public object suggested_sort { get; set; }
        [JsonIgnore]
        public object banned_at_utc { get; set; }
        [JsonIgnore]
        public object view_count { get; set; }
        [JsonIgnore]
        public bool archived { get; set; }
        [JsonIgnore]
        public bool no_follow { get; set; }
        [JsonIgnore]
        public bool is_crosspostable { get; set; }
        [JsonIgnore]
        public bool pinned { get; set; }
        [JsonIgnore]
        public bool over_18 { get; set; }
        [JsonIgnore]
        public Preview preview { get; set; }
        [JsonIgnore]
        public List<object> all_awardings { get; set; }
        [JsonIgnore]
        public List<object> awarders { get; set; }
        [JsonIgnore]
        public bool media_only { get; set; }
        [JsonIgnore]
        public string link_flair_template_id { get; set; }
        [JsonIgnore]
        public bool can_gild { get; set; }
        [JsonIgnore]
        public bool spoiler { get; set; }
        [JsonIgnore]
        public bool locked { get; set; }
        [JsonIgnore]
        public string author_flair_text { get; set; }
        [JsonIgnore]
        public List<object> treatment_tags { get; set; }
        [JsonIgnore]
        public bool visited { get; set; }
        [JsonIgnore]
        public object removed_by { get; set; }
        [JsonIgnore]
        public object num_reports { get; set; }
        [JsonIgnore]
        public object distinguished { get; set; }
        [JsonIgnore]
        public string subreddit_id { get; set; }
        [JsonIgnore]
        public bool author_is_blocked { get; set; }
        [JsonIgnore]
        public object mod_reason_by { get; set; }
        [JsonIgnore]
        public object removal_reason { get; set; }
        [JsonIgnore]
        public string link_flair_background_color { get; set; }
        public string id { get; set; }
        [JsonIgnore]
        public bool is_robot_indexable { get; set; }
        [JsonIgnore]
        public int num_duplicates { get; set; }
        [JsonIgnore]
        public object report_reasons { get; set; }

        public string author { get; set; }
        [JsonIgnore]
        public object discussion_type { get; set; }
        public int num_comments { get; set; }
        [JsonIgnore]
        public bool send_replies { get; set; }
        [JsonIgnore]
        public object media { get; set; }
        [JsonIgnore]
        public bool contest_mode { get; set; }
        [JsonIgnore]
        public bool author_patreon_flair { get; set; }
        [JsonIgnore]
        public string author_flair_text_color { get; set; }
        [JsonIgnore]
        public string permalink { get; set; }
        [JsonIgnore]
        public string whitelist_status { get; set; }
        [JsonIgnore]
        public bool stickied { get; set; }

        public string url { get; set; }
        [JsonIgnore]
        public int subreddit_subscribers { get; set; }

        public long created_utc { get; set; }
        [JsonIgnore]
        public int num_crossposts { get; set; }
        [JsonIgnore]
        public List<object> mod_reports { get; set; }
        [JsonIgnore]
        public bool is_video { get; set; }
        [JsonIgnore]
        public object comment_type { get; set; }
        [JsonIgnore]
        public Replies replies { get; set; }
        [JsonIgnore]
        public object collapsed_reason_code { get; set; }

        public string parent_id { get; set; }
        [JsonIgnore]
        public bool? collapsed { get; set; }

        public string body { get; set; }
        [JsonIgnore]
        public bool? is_submitter { get; set; }
        [JsonIgnore]
        public string body_html { get; set; }
        [JsonIgnore]
        public object collapsed_reason { get; set; }
        [JsonIgnore]
        public object associated_award { get; set; }
        [JsonIgnore]
        public object unrepliable_reason { get; set; }
        [JsonIgnore]
        public bool? score_hidden { get; set; }
        [JsonIgnore]
        public string link_id { get; set; }
        [JsonIgnore]
        public int? controversiality { get; set; }

        public int? depth { get; set; }
        [JsonIgnore]
        public object collapsed_because_crowd_control { get; set; }
    }

    public class Gildings
    {
    }

    public class Image
    {
        public Source source { get; set; }
        [JsonIgnore]
        public List<Resolution> resolutions { get; set; }
        [JsonIgnore]
        public Variants variants { get; set; }
        public string id { get; set; }
    }

    public class MediaEmbed
    {
    }

    public class Preview
    {
        public List<Image> images { get; set; }
        [JsonIgnore]
        public bool enabled { get; set; }
    }

    public class Replies
    {
        [JsonIgnore]
        public string kind { get; set; }
        public Data data { get; set; }
    }

    public class Resolution
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Root
    {
        [JsonIgnore]
        public string kind { get; set; }
        public Data data { get; set; }
    }

    public class SecureMediaEmbed
    {
    }

    public class Source
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Variants
    {
    }

    #endregion

}
