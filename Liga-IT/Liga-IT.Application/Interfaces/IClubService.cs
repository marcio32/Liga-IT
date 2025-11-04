using Liga_IT.Application.DTOs;

namespace Liga_IT.Application.Interfaces
{
    public interface IClubService
    {
        Task<ClubDto> CreateClubAsync(AddClubDto addClubDto);
        Task<bool> DeleteClubAsync(int id);
        Task<IEnumerable<ClubDto>> GetAllClubsAsync();
        Task<ClubDto?> GetClubByIdAsync(int id);
        Task<bool> UpdateClubAsync(UpdateClubDto updateClubDto);
    }
}