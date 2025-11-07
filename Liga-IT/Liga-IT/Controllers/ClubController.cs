using Liga_IT.Application.Interfaces;
using Liga_IT.Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Liga_IT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClubController : ControllerBase
    {
        private readonly IClubService _clubService;

        public ClubController(IClubService clubService)
        {
            _clubService = clubService;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllClubs()
        {
            var clubs = await _clubService.GetAllClubsAsync();
            return Ok(clubs);
        }

        [HttpGet]
        [Route("Get/{id}")]
        public async Task<IActionResult> GetClubById(int id)
        {
            var club = await _clubService.GetClubByIdAsync(id);
            return club == null ? NotFound() : Ok(club);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateClub([FromBody] AddClubDto addClubDto)
        {
            var created = await _clubService.CreateClubAsync(addClubDto);
            return created == null ?  BadRequest() : CreatedAtAction(nameof(GetClubById), new { id = created.Id }, created);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> UpdateClub([FromBody] UpdateClubDto updateClubDto)
        {
            var updated = await _clubService.UpdateClubAsync(updateClubDto);
            return updated == false ?  BadRequest() : Ok(updated);
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var deleted = await _clubService.DeleteClubAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
