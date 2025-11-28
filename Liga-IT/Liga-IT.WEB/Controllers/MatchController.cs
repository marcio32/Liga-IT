using Liga_IT.WEB.Filters;
using Liga_IT.WEB.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace Liga_IT.WEB.Controllers;

[AuthorizeSession]
public class MatchController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly string _apiBaseUrl;

    public MatchController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _apiBaseUrl = _configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7284";
    }

    private async Task<List<ClubViewModel>> GetClubsAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            var response = await client.GetAsync($"{_apiBaseUrl}/api/Club/GetAll");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<ClubViewModel>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<ClubViewModel>();
            }
        }
        catch { }
        return new List<ClubViewModel>();
    }

    private async Task<List<RefereeViewModel>> GetRefereesAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            var response = await client.GetAsync($"{_apiBaseUrl}/api/Referee/GetAll");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<RefereeViewModel>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<RefereeViewModel>();
            }
        }
        catch { }
        return new List<RefereeViewModel>();
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            var response = await client.GetAsync($"{_apiBaseUrl}/api/Match/GetAll");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var matches = JsonSerializer.Deserialize<List<MatchFormViewModel>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<MatchFormViewModel>();

                var clubs = await GetClubsAsync();
                var referees = await GetRefereesAsync();

                foreach (var match in matches)
                {
                    match.HomeClubName = clubs.FirstOrDefault(c => c.Id == match.HomeClubId)?.Name ?? "Desconocido";
                    match.AwayClubName = clubs.FirstOrDefault(c => c.Id == match.AwayClubId)?.Name ?? "Desconocido";
                    if (match.RefereeId.HasValue)
                    {
                        var referee = referees.FirstOrDefault(r => r.Id == match.RefereeId.Value);
                        match.RefereeName = referee != null ? $"{referee.FirstName} {referee.LastName}" : null;
                    }
                }

                return View(matches);
            }

            ViewBag.ErrorMessage = "Error al cargar los partidos";
            return View(new List<MatchFormViewModel>());
        }
        catch
        {
            ViewBag.ErrorMessage = "Error de conexión con la API";
            return View(new List<MatchFormViewModel>());
        }
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Clubs = await GetClubsAsync();
        ViewBag.Referees = await GetRefereesAsync();
        return View(new MatchFormViewModel { MatchDate = DateTime.Now });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MatchFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Clubs = await GetClubsAsync();
            ViewBag.Referees = await GetRefereesAsync();
            return View(model);
        }

        try
        {
            var client = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{_apiBaseUrl}/api/Match/Create", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Partido creado exitosamente";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Clubs = await GetClubsAsync();
            ViewBag.Referees = await GetRefereesAsync();
            ViewBag.ErrorMessage = "Error al crear el partido";
            return View(model);
        }
        catch
        {
            ViewBag.Clubs = await GetClubsAsync();
            ViewBag.Referees = await GetRefereesAsync();
            ViewBag.ErrorMessage = "Error de conexión con la API";
            return View(model);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            var response = await client.GetAsync($"{_apiBaseUrl}/api/Match/Get/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var match = JsonSerializer.Deserialize<MatchFormViewModel>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                ViewBag.Clubs = await GetClubsAsync();
                ViewBag.Referees = await GetRefereesAsync();
                return View(match);
            }

            TempData["ErrorMessage"] = "Partido no encontrado";
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            TempData["ErrorMessage"] = "Error de conexión con la API";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(MatchFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Clubs = await GetClubsAsync();
            ViewBag.Referees = await GetRefereesAsync();
            return View(model);
        }

        try
        {
            var client = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"{_apiBaseUrl}/api/Match/Update", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Partido actualizado exitosamente";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Clubs = await GetClubsAsync();
            ViewBag.Referees = await GetRefereesAsync();
            ViewBag.ErrorMessage = "Error al actualizar el partido";
            return View(model);
        }
        catch
        {
            ViewBag.Clubs = await GetClubsAsync();
            ViewBag.Referees = await GetRefereesAsync();
            ViewBag.ErrorMessage = "Error de conexión con la API";
            return View(model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            var response = await client.DeleteAsync($"{_apiBaseUrl}/api/Match/Delete/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Partido eliminado exitosamente";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al eliminar el partido";
            }
        }
        catch
        {
            TempData["ErrorMessage"] = "Error de conexión con la API";
        }

        return RedirectToAction(nameof(Index));
    }
}
