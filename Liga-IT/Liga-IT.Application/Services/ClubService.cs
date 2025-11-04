using Liga_IT.Application.DTOs;
using Liga_IT.Application.Interfaces;
using Liga_IT.Domain.Entities;
using Liga_IT.Domain.Interfaces;
using Mapster;

namespace Liga_IT.Application.Services;

public class ClubService(IClubRepository clubRepository) : IClubService
{
    public async Task<IEnumerable<ClubDto>> GetAllClubsAsync()
    {
        var clubs = await clubRepository.GetAllAsync();
        return clubs.Select(MapToDto);

    }

    public async Task<ClubDto?> GetClubByIdAsync(int id)
    {
        var club = await clubRepository.GetByIdAsync(id);
        return club != null ? MapToDto(club) : null;
    }


    public async Task<ClubDto> CreateClubAsync(AddClubDto addClubDto)
    {
        var club = addClubDto.Adapt<Club>();
        var createdClub = await clubRepository.AddAsync(club);
        return MapToDto(createdClub);
    }

    public async Task<bool> UpdateClubAsync(UpdateClubDto updateClubDto)
    {
        var existingClub = await clubRepository.GetByIdAsync(updateClubDto.Id);
        if (existingClub == null)
            return false;
        updateClubDto.Adapt(existingClub);
        return await clubRepository.UpdateAsync(existingClub);
    }

    public async Task<bool> DeleteClubAsync(int id)
    {
        var existingClub = await clubRepository.GetByIdAsync(id);
        if (existingClub == null)
            return false;
        return await clubRepository.DeleteAsync(existingClub);
    }

    private static ClubDto MapToDto(Club club) => club.Adapt<ClubDto>();
}