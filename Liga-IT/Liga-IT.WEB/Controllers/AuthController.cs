using Liga_IT.WEB.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace Liga_IT.WEB.Controllers;

public class AuthController(IHttpClientFactory httpClientFactory, IConfiguration configuration) : Controller
{
    public IActionResult Index()
    {
        var errorMessage = HttpContext.Session.GetString("LoginError");
        if (!string.IsNullOrEmpty(errorMessage))
        {
            ViewBag.ErrorMessage = errorMessage;
            HttpContext.Session.Remove("LoginError");
        }
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if(!ModelState.IsValid)
        {
            HttpContext.Session.SetString("LoginError", "Por favor completa todos los campos correctamente");
            return RedirectToAction("Index");
        }

        try
        {
            var client = httpClientFactory.CreateClient();
            var loginData = new { email = model.Email, password = model.Password };
            var json = JsonSerializer.Serialize(loginData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{configuration["ApiSettings:BaseUrl"]}/api/auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var jsonDoc = JsonDocument.Parse(responseContent);
                var token = jsonDoc.RootElement.GetProperty("token").GetString();

                HttpContext.Session.SetString("Token", token ?? "");
                HttpContext.Session.SetString("Email", model.Email);
                HttpContext.Session.SetString("IsAuthenticated", "true");

                return RedirectToAction("Index", "Home");
            }

            HttpContext.Session.SetString("LoginError", "Email o contraseña incorrectos");
        }
        catch (Exception ex)
        {
            HttpContext.Session.SetString("LoginError", "Error al conectar con el servidor");
        }

        return RedirectToAction("Index");
    }

}
