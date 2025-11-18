using Liga_IT.Application.DTOs;
using Liga_IT.Application.Interfaces;
using Liga_IT.Domain.Entities;
using Liga_IT.Domain.Interfaces;
using Mapster;

namespace Liga_IT.Application.Services;

public class ClubService(IClubRepository clubRepository, IRedisCacheService cacheService, ISqsService sqsService) : IClubService
{
    public async Task<IEnumerable<ClubDto>> GetAllClubsAsync()
    {
        var cachedClubs = await cacheService.GetAsync<IEnumerable<ClubDto>>("clubs:all");

        if(cachedClubs != null)
            return cachedClubs;

        var clubs = await clubRepository.GetAllAsync();

        await cacheService.SetAsync("clubs:all", clubs.Select(MapToDto));

        return clubs.Select(MapToDto);

    }

    public async Task<ClubDto?> GetClubByIdAsync(int id)
    {
        var club = await clubRepository.GetByIdAsync(id);
        return club != null ? MapToDto(club) : null;
    }


    public async Task<ClubDto> CreateClubAsync(AddClubDto addClubDto)
    {
        await cacheService.RemoveAsync("clubs:all");
        var club = addClubDto.Adapt<Club>();
        var createdClub = await clubRepository.AddAsync(club);
        await sqsService.SendMessageAsync(new
        {
            EventType = "ClubCreated",
            createdClub,
            TimeStamp = DateTime.UtcNow
        }, QueueNames.ClubQueue);
        return MapToDto(createdClub);
    }

    public async Task<bool> UpdateClubAsync(UpdateClubDto updateClubDto)
    {
        await cacheService.RemoveAsync("clubs:all");

        await sqsService.SendMessageAsync(new
        {
            EventType = "ClubUpdated",
            updateClubDto,
            TimeStamp = DateTime.UtcNow
        }, QueueNames.ClubQueue);

        var existingClub = await clubRepository.GetByIdAsync(updateClubDto.Id);
        if (existingClub == null)
            return false;
        updateClubDto.Adapt(existingClub);
        return await clubRepository.UpdateAsync(existingClub);
    }

    public async Task<bool> DeleteClubAsync(int id)
    {
        await cacheService.RemoveAsync("clubs:all");
        var existingClub = await clubRepository.GetByIdAsync(id);
        if (existingClub == null)
            return false;

        await sqsService.SendMessageAsync(new
        {
            EventType = "ClubDeleted",
            Id = id,
            TimeStamp = DateTime.UtcNow
        }, QueueNames.ClubQueue);
        return await clubRepository.DeleteAsync(existingClub);
    }

    private static ClubDto MapToDto(Club club) => club.Adapt<ClubDto>();
}