using AIResearchService.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AIController(IResearchAgent agent, ISearchRepository repository) : ControllerBase
{
    [HttpGet("start")]
    public async Task<IActionResult> Start()
    {
        var result = await agent.InitAsync();
        return Ok(result.ToString());
    }

    [HttpGet("reset")]
    public async Task<IActionResult> Reset()
    {
        repository.SearchResults.Clear();
        var result = await agent.ResetAsync();
        return Ok(result.ToString());
    }

    [HttpPost("content")]
    public async Task<IActionResult> GetContent([FromBody] string title)
    {
        var result = await agent.GetContent(title);
        return Ok(result.ToString());
    }
    
    [HttpPost("chat")]
    public async Task<IActionResult> Chat([FromBody] string userInput)
    {
        var result = await agent.ChatWithUserAsync(userInput);
        return Ok(result.ToString());
    }

    [HttpGet("repo")]
    public async Task<IActionResult> GetRepositoryList()
    {
        return Ok(repository.SearchResults.GetAllTitlesWithIndex());
    }

}