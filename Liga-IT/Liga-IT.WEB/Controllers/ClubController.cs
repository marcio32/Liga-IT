using Liga_IT.WEB.Filters;
using Liga_IT.WEB.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace Liga_IT.WEB.Controllers;

[AuthorizeSession]
public class ClubController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly string _apiBaseUrl;

    public ClubController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _apiBaseUrl = _configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7284";
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
            var response = await client.GetAsync($"{_apiBaseUrl}/api/Club/GetAll");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var clubs = JsonSerializer.Deserialize<List<ClubFormViewModel>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return View(clubs ?? new List<ClubFormViewModel>());
            }

            ViewBag.ErrorMessage = "Error al cargar los clubes";
            return View(new List<ClubFormViewModel>());
        }
        catch
        {
            ViewBag.ErrorMessage = "Error de conexión con la API";
            return View(new List<ClubFormViewModel>());
        }
    }

    public IActionResult Create()
    {
        return View(new ClubFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ClubFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
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
            var response = await client.PostAsync($"{_apiBaseUrl}/api/Club/Create", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Club creado exitosamente";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ErrorMessage = "Error al crear el club";
            return View(model);
        }
        catch
        {
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
            var response = await client.GetAsync($"{_apiBaseUrl}/api/Club/Get/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var club = JsonSerializer.Deserialize<ClubFormViewModel>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return View(club);
            }

            TempData["ErrorMessage"] = "Club no encontrado";
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
    public async Task<IActionResult> Edit(ClubFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
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
            var response = await client.PutAsync($"{_apiBaseUrl}/api/Club/Update", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Club actualizado exitosamente";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ErrorMessage = "Error al actualizar el club";
            return View(model);
        }
        catch
        {
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
            var response = await client.DeleteAsync($"{_apiBaseUrl}/api/Club/Delete/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Club eliminado exitosamente";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al eliminar el club";
            }
        }
        catch
        {
            TempData["ErrorMessage"] = "Error de conexión con la API";
        }

        return RedirectToAction(nameof(Index));
    }
}
