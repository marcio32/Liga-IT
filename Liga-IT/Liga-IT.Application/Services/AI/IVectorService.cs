namespace Liga_IT.Application.Services.AI
{
    public interface IVectorService
    {
        Task ProcessPdfAsync(string filePath, string documentName);
        Task<string> SearchSimilarAsync(string query, int maxResults = 5);
    }
}