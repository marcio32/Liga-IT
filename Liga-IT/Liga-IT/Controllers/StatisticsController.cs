using Liga_IT.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Liga_IT.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class StatisticsController : ControllerBase
{
    private readonly IStatisticsService _statisticsService;

    public StatisticsController(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    [HttpGet]
    [Route("GetLeagueStatistics")]
    public async Task<IActionResult> GetLeagueStatistics()
    {
        var statistics = await _statisticsService.GetLeagueStatisticsAsync();
        return Ok(statistics);
    }

    [HttpGet]
    [Route("GetMatchStatistics")]
    public async Task<IActionResult> GetMatchStatistics()
    {
        var statistics = await _statisticsService.GetMatchStatisticsAsync();
        return Ok(statistics);
    }

    [HttpGet]
    [Route("GetPlayerStatistics")]
    public async Task<IActionResult> GetPlayerStatistics()
    {
        var statistics = await _statisticsService.GetPlayerStatisticsAsync();
        return Ok(statistics);
    }
}
