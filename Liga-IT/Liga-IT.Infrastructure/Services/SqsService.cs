using Amazon.SQS;
using Amazon.SQS.Model;
using Azure;
using Liga_IT.Application.DTOs;
using Liga_IT.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Liga_IT.Infrastructure.Services;

public class SqsService(IAmazonSQS amazonSQS, ILogger<SqsService> logger) : ISqsService
{
    public async Task<IEnumerable<QueueMessageDto>> ReceiveMessageAsync(string queueName, int maxMessage = 10)
    {
        var queueUrl = await amazonSQS.GetQueueUrlAsync(queueName);

        var request = new ReceiveMessageRequest
        {
            QueueUrl = queueUrl.QueueUrl,
            MaxNumberOfMessages = maxMessage,
            WaitTimeSeconds = 20
        };

        var response = await amazonSQS.ReceiveMessageAsync(request);

        if (response.Messages == null)
            return new List<QueueMessageDto>();

        return response.Messages.Select(m => new QueueMessageDto
        {
            MessageId = m.MessageId,
            Body = m.Body,
            ReceiptHandle = m.ReceiptHandle
        });
    }

    public async Task SendMessageAsync<T>(T message, string queueName, int delaySeconds = 0)
    {
        var queueUrl = await amazonSQS.GetQueueUrlAsync(queueName);
        var messageBody = JsonSerializer.Serialize(message);

        var request = new SendMessageRequest
        {
            QueueUrl = queueUrl.QueueUrl,
            MessageBody = messageBody,
            DelaySeconds = delaySeconds
        };

        var response = await amazonSQS.SendMessageAsync(request);

        logger.LogInformation($"Mensaje enviado a la queue {queueName}. MessageId {response.MessageId}");
    }

    public async Task DeleteMessageAsync(string queueName, string receiptHandle)
    {
        var queueUrl = await amazonSQS.GetQueueUrlAsync(queueName);
        await amazonSQS.DeleteMessageAsync(queueUrl.QueueUrl, receiptHandle);
        logger.LogInformation($"Mensaje eliminado de la queue {queueName}");
    }
}

