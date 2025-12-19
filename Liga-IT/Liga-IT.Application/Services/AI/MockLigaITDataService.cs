using System.Text.Json;

namespace Liga_IT.Application.Services.AI
{
    public class MockLigaITDataService : ILigaITDataService
    {
        public Task<string> GetClubsAsync()
        {
            var clubs = new[]
            {
                new { Id = 1, Name = "Boca Juniors", Stadium = "La Bombonera" },
                new { Id = 2, Name = "River Plate", Stadium = "Monumental" }
            };
            return Task.FromResult(JsonSerializer.Serialize(clubs, new JsonSerializerOptions { WriteIndented = true }));
        }

        public Task<string> GetPlayersAsync(int? clubId = null)
        {
            var players = new[]
            {
                new { Id = 1, Name = "Juan Pérez", ClubId = 1, Position = "Delantero" },
                new { Id = 2, Name = "Carlos López", ClubId = 2, Position = "Mediocampista" }
            };
            return Task.FromResult(JsonSerializer.Serialize(players, new JsonSerializerOptions { WriteIndented = true }));
        }

        public Task<string> GetMatchesAsync(int? clubId = null)
        {
            var matches = new[]
            {
                new { Id = 1, HomeTeam = "Boca Juniors", AwayTeam = "River Plate", Date = DateTime.Now.AddDays(7) }
            };
            return Task.FromResult(JsonSerializer.Serialize(matches, new JsonSerializerOptions { WriteIndented = true }));
        }

        public Task<string> GetRefereesAsync()
        {
            var referees = new[]
            {
                new { Id = 1, Name = "Roberto García", Category = "Primera" }
            };
            return Task.FromResult(JsonSerializer.Serialize(referees, new JsonSerializerOptions { WriteIndented = true }));
        }

        public Task<string> GetStatisticsAsync()
        {
            var stats = new
            {
                Liga = new { TotalClubs = 2, TotalMatches = 10 },
                Partidos = new { Played = 5, Remaining = 5 },
                Jugadores = new { Total = 50, Active = 48 }
            };
            return Task.FromResult(JsonSerializer.Serialize(stats, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}