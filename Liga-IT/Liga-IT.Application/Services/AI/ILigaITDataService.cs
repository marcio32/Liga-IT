namespace Liga_IT.Application.Services.AI
{
    public interface ILigaITDataService
    {
        Task<string> GetClubsAsync();
        Task<string> GetPlayersAsync(int? clubId = null);
        Task<string> GetMatchesAsync(int? clubId = null);
        Task<string> GetRefereesAsync();
        Task<string> GetStatisticsAsync();
    }
}