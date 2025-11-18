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
public class PlayerController : ControllerBase
{
    private readonly IPlayerService _playerService;
    private readonly IValidator<AddPlayerDto> _addValidator;
    private readonly IValidator<UpdatePlayerDto> _updateValidator;

    public PlayerController(IPlayerService playerService, IValidator<AddPlayerDto> addValidator, IValidator<UpdatePlayerDto> updateValidator)
    {
        _playerService = playerService;
        _addValidator = addValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    [Route("GetAll")]
    public async Task<IActionResult> GetAllPlayers()
    {
        var players = await _playerService.GetAllPlayersAsync();
        return Ok(players);
    }

    [HttpGet]
    [Route("Get/{id}")]
    public async Task<IActionResult> GetPlayerById(int id)
    {
        var player = await _playerService.GetPlayerByIdAsync(id);
        return player == null ? NotFound() : Ok(player);
    }

    [HttpGet]
    [Route("GetByClub/{clubId}")]
    public async Task<IActionResult> GetPlayersByClub(int clubId)
    {
        var players = await _playerService.GetPlayersByClubAsync(clubId);
        return Ok(players);
    }

    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> CreatePlayer([FromBody] AddPlayerDto addPlayerDto)
    {
        var validationResult = await _addValidator.ValidateAsync(addPlayerDto);
        return validationResult.IsValid ? StatusCode(201, await _playerService.CreatePlayerAsync(addPlayerDto)) : BadRequest(validationResult.Errors.Adapt<List<ValidationErrorDto>>());
    }

    [HttpPut]
    [Route("Update")]
    public async Task<IActionResult> UpdatePlayer([FromBody] UpdatePlayerDto updatePlayerDto)
    {
        var validationResult = await _updateValidator.ValidateAsync(updatePlayerDto);
        return  validationResult.IsValid ? Ok(await _playerService.UpdatePlayerAsync(updatePlayerDto)) : BadRequest(validationResult.Errors.Adapt<List<ValidationErrorDto>>());
    }

    [HttpDelete]
    [Route("Delete/{id}")]
    public async Task<IActionResult> DeletePlayer(int id)
    {
        var deleted = await _playerService.DeletePlayerAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
