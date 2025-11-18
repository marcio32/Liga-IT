using Liga_IT.Application.DTOs;
using Liga_IT.Application.Interfaces;
using Liga_IT.Domain.Entities;
using Liga_IT.Domain.Interfaces;
using Mapster;

namespace Liga_IT.Application.Services;

public class RefereeService(IRefereeRepository refereeRepository, IRedisCacheService cacheService, ISqsService sqsService) : IRefereeService
{
    public async Task<IEnumerable<RefereeDto>> GetAllRefereesAsync()
    {
        var cachedReferees = await cacheService.GetAsync<IEnumerable<RefereeDto>>("referees:all");

        if (cachedReferees != null)
            return cachedReferees;

        var referees = await refereeRepository.GetAllAsync();

        await cacheService.SetAsync("referees:all", referees.Select(MapToDto));

        return referees.Select(MapToDto);
    }

    public async Task<RefereeDto?> GetRefereeByIdAsync(int id)
    {
        var referee = await refereeRepository.GetByIdAsync(id);
        return referee != null ? MapToDto(referee) : null;
    }

    public async Task<RefereeDto> CreateRefereeAsync(AddRefereeDto addRefereeDto)
    {
        await cacheService.RemoveAsync("referees:all");
        var referee = addRefereeDto.Adapt<Referee>();
        var createdReferee = await refereeRepository.AddAsync(referee);
        await sqsService.SendMessageAsync(new
        {
            EventType = "RefereeCreated",
            createdReferee,
            TimeStamp = DateTime.UtcNow
        }, QueueNames.RefereeQueue);
        return MapToDto(createdReferee);
    }

    public async Task<bool> UpdateRefereeAsync(UpdateRefereeDto updateRefereeDto)
    {
        await cacheService.RemoveAsync("referees:all");

        await sqsService.SendMessageAsync(new
        {
            EventType = "RefereeUpdated",
            updateRefereeDto,
            TimeStamp = DateTime.UtcNow
        }, QueueNames.RefereeQueue);

        var existingReferee = await refereeRepository.GetByIdAsync(updateRefereeDto.Id);
        if (existingReferee == null)
            return false;
        updateRefereeDto.Adapt(existingReferee);
        return await refereeRepository.UpdateAsync(existingReferee);
    }

    public async Task<bool> DeleteRefereeAsync(int id)
    {
        await cacheService.RemoveAsync("referees:all");
        var existingReferee = await refereeRepository.GetByIdAsync(id);
        if (existingReferee == null)
            return false;

        await sqsService.SendMessageAsync(new
        {
            EventType = "RefereeDeleted",
            Id = id,
            TimeStamp = DateTime.UtcNow
        }, QueueNames.RefereeQueue);
        return await refereeRepository.DeleteAsync(existingReferee);
    }

    private static RefereeDto MapToDto(Referee referee) => referee.Adapt<RefereeDto>();
}
