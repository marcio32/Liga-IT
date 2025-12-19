using Microsoft.AspNetCore.Mvc;

namespace Liga_IT.WEB.Controllers
{
    public class AgenteFIFAController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public AgenteFIFAController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Ask([FromBody] QuestionRequest request)
        {
            var baseUrl = _configuration["ApiSettings:APIAIURL"];
            var response = await _httpClient.PostAsJsonAsync($"{baseUrl}/api/AgenteFIFA/ask", request);
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<dynamic>();
                return Json(result);
            }
            
            return Json(new { Answer = "Error al conectar con AgenteFIFA" });
        }
    }

    public record QuestionRequest(string Question);
}