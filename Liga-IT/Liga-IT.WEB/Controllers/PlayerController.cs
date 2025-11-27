using Liga_IT.WEB.Filters;
using Liga_IT.WEB.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace Liga_IT.WEB.Controllers;

[AuthorizeSession]
public class PlayerController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly string _apiBaseUrl;

    public PlayerController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
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
            var response = await client.GetAsync($"{_apiBaseUrl}/api/Player/GetAll");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var players = JsonSerializer.Deserialize<List<PlayerViewModel>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return View(players ?? new List<PlayerViewModel>());
            }

            ViewBag.ErrorMessage = "Error al cargar los jugadores";
            return View(new List<PlayerViewModel>());
        }
        catch
        {
            ViewBag.ErrorMessage = "Error de conexión con la API";
            return View(new List<PlayerViewModel>());
        }
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Clubs = await GetClubsAsync();
        return View(new PlayerViewModel { DateOfBirth = DateTime.Now.AddYears(-20), JoinedClubDate = DateTime.Now });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PlayerViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Clubs = await GetClubsAsync();
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
            var response = await client.PostAsync($"{_apiBaseUrl}/api/Player/Create", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Jugador creado exitosamente";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Clubs = await GetClubsAsync();
            ViewBag.ErrorMessage = "Error al crear el jugador";
            return View(model);
        }
        catch
        {
            ViewBag.Clubs = await GetClubsAsync();
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
            var response = await client.GetAsync($"{_apiBaseUrl}/api/Player/Get/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var player = JsonSerializer.Deserialize<PlayerViewModel>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                ViewBag.Clubs = await GetClubsAsync();
                return View(player);
            }

            TempData["ErrorMessage"] = "Jugador no encontrado";
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
    public async Task<IActionResult> Edit(PlayerViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Clubs = await GetClubsAsync();
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
            var response = await client.PutAsync($"{_apiBaseUrl}/api/Player/Update", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Jugador actualizado exitosamente";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Clubs = await GetClubsAsync();
            ViewBag.ErrorMessage = "Error al actualizar el jugador";
            return View(model);
        }
        catch
        {
            ViewBag.Clubs = await GetClubsAsync();
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
            var response = await client.DeleteAsync($"{_apiBaseUrl}/api/Player/Delete/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Jugador eliminado exitosamente";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al eliminar el jugador";
            }
        }
        catch
        {
            TempData["ErrorMessage"] = "Error de conexión con la API";
        }

        return RedirectToAction(nameof(Index));
    }
}
