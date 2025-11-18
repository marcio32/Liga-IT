using Liga_IT.Application.Interfaces;
using Liga_IT.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FluentValidation;
using Mapster;

namespace Liga_IT.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MatchController : ControllerBase
{
    private readonly IMatchService _matchService;
    private readonly IValidator<AddMatchDto> _addValidator;
    private readonly IValidator<UpdateMatchDto> _updateValidator;

    public MatchController(IMatchService matchService, IValidator<AddMatchDto> addValidator, IValidator<UpdateMatchDto> updateValidator)
    {
        _matchService = matchService;
        _addValidator = addValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    [Route("GetAll")]
    public async Task<IActionResult> GetAllMatches()
    {
        var matches = await _matchService.GetAllMatchesAsync();
        return Ok(matches);
    }

    [HttpGet]
    [Route("Get/{id}")]
    public async Task<IActionResult> GetMatchById(int id)
    {
        var match = await _matchService.GetMatchByIdAsync(id);
        return match == null ? NotFound() : Ok(match);
    }

    [HttpGet]
    [Route("GetByClub/{clubId}")]
    public async Task<IActionResult> GetMatchesByClub(int clubId)
    {
        var matches = await _matchService.GetMatchesByClubAsync(clubId);
        return Ok(matches);
    }

    [HttpGet]
    [Route("GetByRound/{round}")]
    public async Task<IActionResult> GetMatchesByRound(int round)
    {
        var matches = await _matchService.GetMatchesByRoundAsync(round);
        return Ok(matches);
    }

    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> CreateMatch([FromBody] AddMatchDto addMatchDto)
    {
        var validationResult = await _addValidator.ValidateAsync(addMatchDto);
        return  validationResult.IsValid ? StatusCode(201, await _matchService.CreateMatchAsync(addMatchDto)) : BadRequest(validationResult.Errors.Adapt<List<ValidationErrorDto>>());
    }

    [HttpPut]
    [Route("Update")]
    public async Task<IActionResult> UpdateMatch([FromBody] UpdateMatchDto updateMatchDto)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateMatchDto);
        return validationResult.IsValid ? Ok(await _matchService.UpdateMatchAsync(updateMatchDto)) : BadRequest(validationResult.Errors.Adapt<List<ValidationErrorDto>>());
    }

    [HttpDelete]
    [Route("Delete/{id}")]
    public async Task<IActionResult> DeleteMatch(int id)
    {
        var deleted = await _matchService.DeleteMatchAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
