using Liga_IT.Application.Services.AI;
using Microsoft.AspNetCore.Mvc;

namespace Liga_IT.AI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgenteFIFAController : ControllerBase
    {
        private readonly AIService _aiService;
        private readonly IVectorService _vectorService;

        public AgenteFIFAController(AIService aiService, IVectorService vectorService)
        {
            _aiService = aiService;
            _vectorService = vectorService;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> AskQuestion([FromBody] QuestionRequest request)
        {
            var answer = await _aiService.AskAsync(request.Question);
            return Ok(new { Answer = answer });
        }

        [HttpPost("upload-document")]
        public async Task<IActionResult> UploadDocument(IFormFile file, [FromForm] string documentName)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No se proporcionó archivo");

            if (!file.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                return BadRequest("Solo se permiten archivos PDF");

            var tempPath = Path.GetTempFileName();
            try
            {
                using (var stream = new FileStream(tempPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                await _vectorService.ProcessPdfAsync(tempPath, documentName);
                return Ok(new { Message = "Documento procesado exitosamente" });
            }
            finally
            {
                if (System.IO.File.Exists(tempPath))
                    System.IO.File.Delete(tempPath);
            }
        }
    }

    public record QuestionRequest(string Question);
}
