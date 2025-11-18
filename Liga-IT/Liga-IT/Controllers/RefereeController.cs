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
public class RefereeController : ControllerBase
{
    private readonly IRefereeService _refereeService;
    private readonly IValidator<AddRefereeDto> _addValidator;
    private readonly IValidator<UpdateRefereeDto> _updateValidator;

    public RefereeController(IRefereeService refereeService, IValidator<AddRefereeDto> addValidator, IValidator<UpdateRefereeDto> updateValidator)
    {
        _refereeService = refereeService;
        _addValidator = addValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    [Route("GetAll")]
    public async Task<IActionResult> GetAllReferees()
    {
        var referees = await _refereeService.GetAllRefereesAsync();
        return Ok(referees);
    }

    [HttpGet]
    [Route("Get/{id}")]
    public async Task<IActionResult> GetRefereeById(int id)
    {
        var referee = await _refereeService.GetRefereeByIdAsync(id);
        return referee == null ? NotFound() : Ok(referee);
    }

    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> CreateReferee([FromBody] AddRefereeDto addRefereeDto)
    {
        var validationResult = await _addValidator.ValidateAsync(addRefereeDto);
        return validationResult.IsValid ? StatusCode(201, await _refereeService.CreateRefereeAsync(addRefereeDto)) : BadRequest(validationResult.Errors.Adapt<List<ValidationErrorDto>>());
    }

    [HttpPut]
    [Route("Update")]
    public async Task<IActionResult> UpdateReferee([FromBody] UpdateRefereeDto updateRefereeDto)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateRefereeDto);
        return  validationResult.IsValid ? Ok(await _refereeService.UpdateRefereeAsync(updateRefereeDto)) : BadRequest(validationResult.Errors.Adapt<List<ValidationErrorDto>>());
    }

    [HttpDelete]
    [Route("Delete/{id}")]
    public async Task<IActionResult> DeleteReferee(int id)
    {
        var deleted = await _refereeService.DeleteRefereeAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
