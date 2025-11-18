using Liga_IT.Application.DTOs;

namespace Liga_IT.Application.Interfaces;

public interface IRefereeService
{
    Task<RefereeDto> CreateRefereeAsync(AddRefereeDto addRefereeDto);
    Task<bool> DeleteRefereeAsync(int id);
    Task<IEnumerable<RefereeDto>> GetAllRefereesAsync();
    Task<RefereeDto?> GetRefereeByIdAsync(int id);
    Task<bool> UpdateRefereeAsync(UpdateRefereeDto updateRefereeDto);
}
