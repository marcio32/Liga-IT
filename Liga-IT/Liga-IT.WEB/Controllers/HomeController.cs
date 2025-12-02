using Liga_IT.WEB.Filters;
using Liga_IT.WEB.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Liga_IT.WEB.Controllers
{
    [AuthorizeSession]
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly string _apiBaseUrl;

        public HomeController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _apiBaseUrl = _configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7284";
        }

        public async Task<IActionResult> Index()
        {
            var dashboard = new DashboardViewModel();
            
            try
            {
                var client = _httpClientFactory.CreateClient();
                var token = HttpContext.Session.GetString("Token");
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                var playersResponse = await client.GetAsync($"{_apiBaseUrl}/api/Player/GetAll");
                if (playersResponse.IsSuccessStatusCode)
                {
                    var content = await playersResponse.Content.ReadAsStringAsync();
                    var players = JsonSerializer.Deserialize<List<PlayerViewModel>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    dashboard.TotalPlayers = players?.Count ?? 0;
                    dashboard.ActivePlayers = players?.Count(p => p.IsActive) ?? 0;
                    dashboard.TotalGoals = players?.Sum(p => p.Goals) ?? 0;
                }

                var clubsResponse = await client.GetAsync($"{_apiBaseUrl}/api/Club/GetAll");
                if (clubsResponse.IsSuccessStatusCode)
                {
                    var content = await clubsResponse.Content.ReadAsStringAsync();
                    var clubs = JsonSerializer.Deserialize<List<ClubViewModel>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    dashboard.TotalClubs = clubs?.Count ?? 0;
                }
            }
            catch { }

            return View(dashboard);
        }

        public IActionResult Chat()
        {
            return View();
        }
    }
}
