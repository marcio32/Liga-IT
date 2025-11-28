using Liga_IT.WEB.Filters;
using Liga_IT.WEB.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace Liga_IT.WEB.Controllers;

[AuthorizeSession]
public class RefereeController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly string _apiBaseUrl;

    public RefereeController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
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
            var response = await client.GetAsync($"{_apiBaseUrl}/api/Referee/GetAll");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var referees = JsonSerializer.Deserialize<List<RefereeViewModel>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return View(referees ?? new List<RefereeViewModel>());
            }

            ViewBag.ErrorMessage = "Error al cargar los árbitros";
            return View(new List<RefereeViewModel>());
        }
        catch
        {
            ViewBag.ErrorMessage = "Error de conexión con la API";
            return View(new List<RefereeViewModel>());
        }
    }

    public IActionResult Create()
    {
        return View(new RefereeViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RefereeViewModel model)
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
            var response = await client.PostAsync($"{_apiBaseUrl}/api/Referee/Create", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Árbitro creado exitosamente";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ErrorMessage = "Error al crear el árbitro";
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
            var response = await client.GetAsync($"{_apiBaseUrl}/api/Referee/Get/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var referee = JsonSerializer.Deserialize<RefereeViewModel>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return View(referee);
            }

            TempData["ErrorMessage"] = "Árbitro no encontrado";
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
    public async Task<IActionResult> Edit(RefereeViewModel model)
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
            var response = await client.PutAsync($"{_apiBaseUrl}/api/Referee/Update", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Árbitro actualizado exitosamente";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ErrorMessage = "Error al actualizar el árbitro";
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
            var response = await client.DeleteAsync($"{_apiBaseUrl}/api/Referee/Delete/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Árbitro eliminado exitosamente";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al eliminar el árbitro";
            }
        }
        catch
        {
            TempData["ErrorMessage"] = "Error de conexión con la API";
        }

        return RedirectToAction(nameof(Index));
    }
}
