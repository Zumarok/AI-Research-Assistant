namespace AIResearchService.Common;

/// <summary>
/// Time frames to limit google search result.
/// </summary>
public enum TimeFrame
{
    All,
    PastHour,
    PastDay,
    PastWeek,
    PastMonth,
    PastYear
}

/// <summary>
/// Specific sites to Google search.
/// </summary>
public enum Site
{
    None,
    YouTube,
    Reddit,
    Wikipedia,
    News
}
