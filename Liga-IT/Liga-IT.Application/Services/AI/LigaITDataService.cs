using Liga_IT.Application.Interfaces;
using System.Text.Json;
using System.Text;

namespace Liga_IT.Application.Services.AI
{
    public class LigaITDataService : ILigaITDataService
    {
        private readonly IClubService _clubService;
        private readonly IPlayerService _playerService;
        private readonly IMatchService _matchService;
        private readonly IRefereeService _refereeService;
        private readonly IStatisticsService _statisticsService;

        public LigaITDataService(
            IClubService clubService,
            IPlayerService playerService,
            IMatchService matchService,
            IRefereeService refereeService,
            IStatisticsService statisticsService)
        {
            _clubService = clubService;
            _playerService = playerService;
            _matchService = matchService;
            _refereeService = refereeService;
            _statisticsService = statisticsService;
        }

        public async Task<string> GetClubsAsync()
        {
            var clubs = await _clubService.GetAllClubsAsync();
            return JsonSerializer.Serialize(clubs, new JsonSerializerOptions { WriteIndented = true });
        }

        public async Task<string> GetPlayersAsync(int? clubId = null)
        {
            var players = clubId.HasValue 
                ? await _playerService.GetPlayersByClubAsync(clubId.Value)
                : await _playerService.GetAllPlayersAsync();
            return JsonSerializer.Serialize(players, new JsonSerializerOptions { WriteIndented = true });
        }

        public async Task<string> GetMatchesAsync(int? clubId = null)
        {
            var matches = clubId.HasValue
                ? await _matchService.GetMatchesByClubAsync(clubId.Value)
                : await _matchService.GetAllMatchesAsync();
            return JsonSerializer.Serialize(matches, new JsonSerializerOptions { WriteIndented = true });
        }

        public async Task<string> GetRefereesAsync()
        {
            var referees = await _refereeService.GetAllRefereesAsync();
            return JsonSerializer.Serialize(referees, new JsonSerializerOptions { WriteIndented = true });
        }

        public async Task<string> GetStatisticsAsync()
        {
            var result = new StringBuilder();
            
            var leagueStats = await _statisticsService.GetLeagueStatisticsAsync();
            var matchStats = await _statisticsService.GetMatchStatisticsAsync();
            var playerStats = await _statisticsService.GetPlayerStatisticsAsync();
            
            result.AppendLine("Liga: " + JsonSerializer.Serialize(leagueStats));
            result.AppendLine("Partidos: " + JsonSerializer.Serialize(matchStats));
            result.AppendLine("Jugadores: " + JsonSerializer.Serialize(playerStats));
            
            return result.ToString();
        }


    }
}