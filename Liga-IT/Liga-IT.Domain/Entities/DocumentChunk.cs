namespace Liga_IT.Domain.Entities
{
    public class DocumentChunk
    {
        public int Id { get; set; }
        public string? DocumentName { get; set; }
        public string? ChunkText { get; set; }
        public byte[]? Embedding { get; set; }
        public int? ChunkIndex { get; set; }
    }
}