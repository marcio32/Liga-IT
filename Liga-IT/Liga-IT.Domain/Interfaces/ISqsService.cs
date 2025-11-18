using System.Reflection.Metadata;

namespace Liga_IT.Domain.Interfaces
{
    public interface ISqsService
    {
        Task DeleteMessageAsync(string queueName, string receiptHandle);
        Task<IEnumerable<QueueMessageDto>> ReceiveMessageAsync(string queueName, int maxMessage = 10);
        Task SendMessageAsync<T>(T message, string queueName, int delaySeconds = 0);
    }

    public class QueueMessageDto
    {
        public string MessageId { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string ReceiptHandle { get; set; } = string.Empty;
    }

    public static class QueueNames
    {
        public const string ClubQueue = "Club-events";
        public const string RefereeQueue = "Referee-events";
        public const string MatchQueue = "Match-events";
        public const string PlayerQueue = "Player-events";
    }
}