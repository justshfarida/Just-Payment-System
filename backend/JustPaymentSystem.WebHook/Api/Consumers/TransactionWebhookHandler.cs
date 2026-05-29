using Api.DTOs;
using Api.Events;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Wolverine;

namespace JustPaymentSystem.WebHook.Consumers;

public class TransactionWebhookHandler
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<TransactionWebhookHandler> _logger;
    private readonly IMessageBus _bus;

    public TransactionWebhookHandler(IHttpClientFactory httpClientFactory, ILogger<TransactionWebhookHandler> logger, IMessageBus bus)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _bus = bus;
    }

    // THIS RUNS AUTOMATICALLY WHEN A SUCCESS MESSAGE ARRIVES FROM YOUR FRIEND'S TRANSACTIONS SERVICE
    public async Task Handle(TransactionCompletedIntegrationEvent message)
    {
        // Guard Clause: Safely skip empty or malformed message data properties
        if (message == null || message.MerchantId == null)
        {
            _logger.LogWarning("Discarding invalid success message payload with empty MerchantId.");
            return;
        }

        _logger.LogInformation("Starting webhook processing for Transaction: {TxId}", message.TransactionId);

        string eventTypeName = "payment.success";
        var httpClient = _httpClientFactory.CreateClient();

        try
        {
            // Fetch Webhook URL and SecretKey dynamically from the Merchant Service database
            var merchantSettingsUrl = $"https://localhost:7041/api/merchants/{message.MerchantId}/webhooks?eventType={eventTypeName}";
            var settingsResponse = await httpClient.GetFromJsonAsync<MerchantWebhookSettingsDto>(merchantSettingsUrl);

            if (settingsResponse == null || string.IsNullOrEmpty(settingsResponse.WebhookUrl))
            {
                throw new Exception($"No active webhook configuration found for Merchant Guid: {message.MerchantId}");
            }

            // Build the official payload payload format the external merchant expects
            var webhookPayload = new
            {
                @event = eventTypeName,
                transactionId = message.TransactionId,
                merchantId = message.MerchantId,
                orderId = message.OrderId,
                amount = message.Amount,
                currency = message.Currency,
                timestamp = message.Timestamp
            };

            var jsonPayload = JsonSerializer.Serialize(webhookPayload);
            string signature = ComputeHmacSha256(jsonPayload, settingsResponse.SecretKey);

            // Construct the HTTP POST message wrapper
            var request = new HttpRequestMessage(HttpMethod.Post, settingsResponse.WebhookUrl);
            request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            request.Headers.Add("X-Webhook-Signature", signature);

            _logger.LogInformation("Dispatching success webhook to External Merchant API: {Url}", settingsResponse.WebhookUrl);
            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"External Merchant server rejected notification with status code: {response.StatusCode}");
            }

            _logger.LogInformation("Webhook successfully delivered for Transaction: {TxId}", message.TransactionId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to deliver success webhook network request. Alerting Transactions service...");

            // Deliver the callback-failed alert message to your teammate's queue
            await _bus.PublishAsync(new WebhookCallbackFailedIntegrationEvent
            {
                TransactionId = message.TransactionId,
                MerchantId = message.MerchantId.ToString(),
                OrderId = message.OrderId,
                Reason = $"Network delivery exception: {ex.Message}",
                Timestamp = DateTime.UtcNow
            });
        }
    }

    /// <summary>
    /// Computes an HMAC-SHA256 hash to create a secure, verifiable signature.
    /// </summary>
    private static string ComputeHmacSha256(string data, string secretKey)
    {
        var keyBytes = Encoding.UTF8.GetBytes(secretKey);
        var dataBytes = Encoding.UTF8.GetBytes(data);

        using var hmac = new HMACSHA256(keyBytes);
        var hashBytes = hmac.ComputeHash(dataBytes);

        return Convert.ToHexString(hashBytes).ToLower();
    }
}