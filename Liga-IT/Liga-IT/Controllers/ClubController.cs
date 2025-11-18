using Liga_IT.Application.Interfaces;
using Liga_IT.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FluentValidation;
using Mapster;

namespace Liga_IT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClubController(IClubService clubService, IValidator<AddClubDto> createValidator, IValidator<UpdateClubDto> updateValidator) : ControllerBase
    {

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllClubs()
        {
            var clubs = await clubService.GetAllClubsAsync();
            return Ok(clubs);
        }

        [HttpGet]
        [Route("Get/{id}")]
        public async Task<IActionResult> GetClubById(int id)
        {
            var club = await clubService.GetClubByIdAsync(id);
            return club == null ? NotFound() : Ok(club);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateClub([FromBody] AddClubDto addClubDto)
        {
            var validationResult = await createValidator.ValidateAsync(addClubDto);
            return validationResult.IsValid ? StatusCode(201, await clubService.CreateClubAsync(addClubDto)) : BadRequest(validationResult.Errors.Adapt<List<ValidationErrorDto>>());
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> UpdateClub([FromBody] UpdateClubDto updateClubDto)
        {
            var validationResult = await updateValidator.ValidateAsync(updateClubDto);
            return validationResult.IsValid ? Ok(await clubService.UpdateClubAsync(updateClubDto)) : BadRequest(validationResult.Errors.Adapt<List<ValidationErrorDto>>());
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var deleted = await clubService.DeleteClubAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
