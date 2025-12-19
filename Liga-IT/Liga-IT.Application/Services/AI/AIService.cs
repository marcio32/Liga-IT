using OpenAI.Chat;
using System.Text;
using System.Text.Json;

namespace Liga_IT.Application.Services.AI
{
    public class AIService
    {
        private readonly ChatClient _client;
        private readonly ILigaITDataService _dataService;
        private readonly IVectorService _vectorService;
        private readonly string _systemPrompt = @"
Eres un asesor experto en fútbol argentino para Liga Libre.

Tienes acceso a funciones para consultar la base de datos de Liga IT:
- get_clubs: Obtiene información de clubes
- get_players: Obtiene información de jugadores (opcional: por club)
- get_matches: Obtiene información de partidos (opcional: por club)
- get_referees: Obtiene información de árbitros
- get_statistics: Obtiene estadísticas generales
- search_documents: Busca información en documentos vectorizados

Usa estas funciones según lo que necesites para responder la pregunta del usuario.

Tu personalidad:
- Apasionado por el fútbol argentino
- Conocedor profundo y analítico
- Amigable, usando lenguaje futbolístico argentino

FORMATO DE RESPUESTA:
- Responde DIRECTAMENTE en HTML, sin bloques de código markdown
- NO uses ```html ni ``` en tu respuesta
- Usa <strong> para texto en negrita
- Usa <em> para énfasis
- Usa <ul> y <li> para listas
- Usa <h3> para subtítulos
- Usa <p> para párrafos
- Estructura la información de manera clara y organizada

Siempre responde en español argentino con HTML directo (sin markdown).";

        public AIService(string apiKey, ILigaITDataService dataService, IVectorService vectorService)
        {
            _client = new ChatClient("gpt-4o-mini", apiKey);
            _dataService = dataService;
            _vectorService = vectorService;
        }

        public async Task<string> AskAsync(string question)
        {
            var tools = GetChatTools();
            var options = new ChatCompletionOptions();
            foreach (var tool in tools)
            {
                options.Tools.Add(tool);
            }

            var messages = new List<ChatMessage>
            {
                new SystemChatMessage(_systemPrompt),
                new UserChatMessage(question)
            };

            var response = await _client.CompleteChatAsync(messages, options);
            var message = response.Value;

            if (message.ToolCalls?.Count > 0)
            {
                messages.Add(new AssistantChatMessage(message));
                
                foreach (var toolCall in message.ToolCalls)
                {
                    var functionResult = await ExecuteFunctionAsync(toolCall.FunctionName, toolCall.FunctionArguments);
                    messages.Add(new ToolChatMessage(toolCall.Id, functionResult));
                }

                var finalResponse = await _client.CompleteChatAsync(messages);
                return CleanHtmlResponse(finalResponse.Value.Content[0].Text);
            }

            return CleanHtmlResponse(message.Content[0].Text);
        }

        private List<ChatTool> GetChatTools()
        {
            return new List<ChatTool>
            {
                ChatTool.CreateFunctionTool(
                    "get_clubs",
                    "Obtiene información de todos los clubes registrados"),
                ChatTool.CreateFunctionTool(
                    "get_players",
                    "Obtiene información de jugadores",
                    BinaryData.FromString(@"{
                        ""type"": ""object"",
                        ""properties"": {
                            ""clubId"": {
                                ""type"": ""integer"",
                                ""description"": ""ID del club para filtrar jugadores (opcional)""
                            }
                        }
                    }")),
                ChatTool.CreateFunctionTool(
                    "get_matches",
                    "Obtiene información de partidos",
                    BinaryData.FromString(@"{
                        ""type"": ""object"",
                        ""properties"": {
                            ""clubId"": {
                                ""type"": ""integer"",
                                ""description"": ""ID del club para filtrar partidos (opcional)""
                            }
                        }
                    }")),
                ChatTool.CreateFunctionTool(
                    "get_referees",
                    "Obtiene información de todos los árbitros"),
                ChatTool.CreateFunctionTool(
                    "get_statistics",
                    "Obtiene estadísticas generales de la liga"),
                ChatTool.CreateFunctionTool(
                    "search_documents",
                    "Busca información en documentos vectorizados",
                    BinaryData.FromString(@"{
                        ""type"": ""object"",
                        ""properties"": {
                            ""query"": {
                                ""type"": ""string"",
                                ""description"": ""Consulta para buscar en documentos""
                            }
                        },
                        ""required"": [""query""]
                    }"))
            };
        }

        private async Task<string> ExecuteFunctionAsync(string functionName, BinaryData arguments)
        {
            var args = JsonSerializer.Deserialize<Dictionary<string, object>>(arguments.ToString()) ?? new();

            return functionName switch
            {
                "get_clubs" => await _dataService.GetClubsAsync(),
                "get_players" => await _dataService.GetPlayersAsync(GetIntValue(args, "clubId")),
                "get_matches" => await _dataService.GetMatchesAsync(GetIntValue(args, "clubId")),
                "get_referees" => await _dataService.GetRefereesAsync(),
                "get_statistics" => await _dataService.GetStatisticsAsync(),
                "search_documents" => await _vectorService.SearchSimilarAsync(GetStringValue(args, "query")),
                _ => "Función no encontrada"
            };
        }

        private static int? GetIntValue(Dictionary<string, object> args, string key)
        {
            if (args.TryGetValue(key, out var value) && value != null)
            {
                if (value is JsonElement element && element.TryGetInt32(out var intValue))
                    return intValue;
                if (int.TryParse(value.ToString(), out var parsedValue))
                    return parsedValue;
            }
            return null;
        }

        private static string GetStringValue(Dictionary<string, object> args, string key)
        {
            if (args.TryGetValue(key, out var value) && value != null)
            {
                if (value is JsonElement element && element.ValueKind == JsonValueKind.String)
                    return element.GetString() ?? "";
                return value.ToString() ?? "";
            }
            return "";
        }

        private static string CleanHtmlResponse(string response)
        {
            if (string.IsNullOrEmpty(response))
                return response;

            // Remover bloques de código markdown que envuelven HTML
            var cleaned = response.Trim();
            
            // Remover ```html al inicio
            if (cleaned.StartsWith("```html"))
                cleaned = cleaned.Substring(7).TrimStart();
            else if (cleaned.StartsWith("```"))
                cleaned = cleaned.Substring(3).TrimStart();
            
            // Remover ``` al final
            if (cleaned.EndsWith("```"))
                cleaned = cleaned.Substring(0, cleaned.Length - 3).TrimEnd();
            
            return cleaned;
        }
    }
}
