using Liga_IT.Domain.Entities;
using Liga_IT.Infrastructure.Data;
using Liga_IT.Application.Services.AI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenAI.Embeddings;
using System.Text;
using System.Text.Json;
using UglyToad.PdfPig;

namespace Liga_IT.Infrastructure.Services
{
    public class VectorService : IVectorService
    {
        private readonly ApplicationDbContext _context;
        private readonly EmbeddingClient _embeddingClient;

        public VectorService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            var apiKey = configuration["OpenAI:ApiKey"] ?? "your-openai-api-key";
            _embeddingClient = new EmbeddingClient("text-embedding-3-small", apiKey);
        }

        public async Task ProcessPdfAsync(string filePath, string documentName)
        {
            var text = ExtractTextFromPdf(filePath);
            var chunks = SplitIntoChunks(text, 1000);

            for (int i = 0; i < chunks.Count; i++)
            {
                var embedding = await GetEmbeddingAsync(chunks[i]);
                var chunk = new DocumentChunk
                {
                    DocumentName = documentName,
                    ChunkText = chunks[i],
                    Embedding = embedding,
                    ChunkIndex = i
                };

                _context.Set<DocumentChunk>().Add(chunk);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<string> SearchSimilarAsync(string query, int maxResults = 5)
        {
            var queryEmbedding = await GetEmbeddingAsync(query);
            var queryVector = DeserializeEmbedding(queryEmbedding);

            var chunks = await _context.Set<DocumentChunk>()
                .Where(c => c.Embedding != null)
                .ToListAsync();

            var similarities = chunks.Select(chunk => new
            {
                Chunk = chunk,
                Similarity = CosineSimilarity(queryVector, DeserializeEmbedding(chunk.Embedding!))
            })
            .OrderByDescending(x => x.Similarity)
            .Take(maxResults)
            .ToList();

            var result = new StringBuilder();
            foreach (var item in similarities)
            {
                result.AppendLine($"Documento: {item.Chunk.DocumentName}");
                result.AppendLine($"Contenido: {item.Chunk.ChunkText}");
                result.AppendLine($"Similitud: {item.Similarity:F3}");
                result.AppendLine("---");
            }

            return result.ToString();
        }

        private string ExtractTextFromPdf(string filePath)
        {
            using var document = PdfDocument.Open(filePath);
            var text = new StringBuilder();
            
            foreach (var page in document.GetPages())
            {
                text.AppendLine(page.Text);
            }
            
            return text.ToString();
        }

        private List<string> SplitIntoChunks(string text, int chunkSize)
        {
            var chunks = new List<string>();
            var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            
            for (int i = 0; i < words.Length; i += chunkSize)
            {
                var chunk = string.Join(" ", words.Skip(i).Take(chunkSize));
                chunks.Add(chunk);
            }
            
            return chunks;
        }

        private async Task<byte[]> GetEmbeddingAsync(string text)
        {
            var response = await _embeddingClient.GenerateEmbeddingAsync(text);
            var embedding = response.Value.ToFloats().ToArray();
            return SerializeEmbedding(embedding);
        }

        private byte[] SerializeEmbedding(float[] embedding)
        {
            return JsonSerializer.SerializeToUtf8Bytes(embedding);
        }

        private float[] DeserializeEmbedding(byte[] data)
        {
            return JsonSerializer.Deserialize<float[]>(data) ?? Array.Empty<float>();
        }

        private double CosineSimilarity(float[] a, float[] b)
        {
            var dotProduct = a.Zip(b, (x, y) => x * y).Sum();
            var magnitudeA = Math.Sqrt(a.Sum(x => x * x));
            var magnitudeB = Math.Sqrt(b.Sum(x => x * x));
            return dotProduct / (magnitudeA * magnitudeB);
        }
    }
}