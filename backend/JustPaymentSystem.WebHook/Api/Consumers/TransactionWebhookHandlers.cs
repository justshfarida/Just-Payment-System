using Api.DTOs;
using Api.Events;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace JustPaymentSystem.WebHook.Consumers;

public class TransactionWebhookHandlers
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TransactionWebhookHandlers> _logger;

    public TransactionWebhookHandlers(HttpClient httpClient, ILogger<TransactionWebhookHandlers> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    // 1. THIS RUNS AUTOMATICALLY WHEN A SUCCESS MESSAGE ARRIVES
    public async Task Handle(TransactionCompletedIntegrationEvent message)
    {
        _logger.LogInformation("Starting webhook processing for Transaction: {TxId}", message.TransactionId);

        string eventTypeName = "payment.success";

        // 1. Fetch Webhook URL and SecretKey dynamically from the Merchant Service
        // Note: Adjust the port and base address to match your Merchant API gateway routing profile
        var merchantSettingsUrl = $"http://localhost:5019/api/merchants/{message.MerchantId}/webhooks?eventType={eventTypeName}";
        var settingsResponse = await _httpClient.GetFromJsonAsync<MerchantWebhookSettingsDto>(merchantSettingsUrl);

        if (settingsResponse == null || string.IsNullOrEmpty(settingsResponse.WebhookUrl))
        {
            _logger.LogWarning("No active webhook configuration found for Merchant: {MerchantId}", message.MerchantId);
            return;
        }

        // 2. Build the official payload that the external merchant expects
        var webhookPayload = new
        {
            @event = eventTypeName,
            transactionId = message.TransactionId,
            merchantId = message.MerchantId,
            orderId = message.OrderId,
            amount = message.Amount,
            currency = message.Currency,
            successRedirectUrl = message.SuccessRedirectUrl,
            errorRedirectUrl = message.ErrorRedirectUrl,
            timestamp = message.Timestamp
        };

        // 3. Serialize to a raw string (Required for calculating the signature and transmitting the body)
        var jsonPayload = JsonSerializer.Serialize(webhookPayload);

        // 4. Compute the HMAC-SHA256 signature using the Merchant's Secret Key
        string signature = ComputeHmacSha256(jsonPayload, settingsResponse.SecretKey);

        // 5. Construct the HTTP Request message
        var request = new HttpRequestMessage(HttpMethod.Post, settingsResponse.WebhookUrl);
        request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        // Append the signature to the custom header so the merchant can verify payload integrity
        request.Headers.Add("X-Webhook-Signature", signature);

        _logger.LogInformation("Dispatching webhook to External Merchant API: {Url}", settingsResponse.WebhookUrl);

        // 6. Transmit the payload
        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"External Merchant API rejected webhook with status code: {response.StatusCode}");
        }

        _logger.LogInformation("Webhook successfully delivered for Transaction: {TxId}", message.TransactionId);
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

        // Convert byte array to a lowercase hexadecimal string (industry standard format)
        return Convert.ToHexString(hashBytes).ToLower();
    }
    // 2. THIS RUNS AUTOMATICALLY WHEN A FAILURE MESSAGE ARRIVES
    public async Task Handle(TransactionFailedIntegrationEvent message)
    {
        _logger.LogWarning("Processing failure webhook for Transaction: {TxId}. Reason: {Reason}", message.TransactionId, message.Reason);

        string merchantCallbackUrl = "https://mock-merchant-shop.com/api/payment-callback";

        var webhookPayload = new
        {
            @event = "payment.failed",
            transactionId = message.TransactionId,
            orderId = message.OrderId,
            failureReason = message.Reason,
            timestamp = message.Timestamp
        };

        var response = await _httpClient.PostAsJsonAsync(merchantCallbackUrl, webhookPayload);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Merchant returned failed status code on failure event: {response.StatusCode}");
        }

        _logger.LogInformation("Successfully delivered failure webhook for Transaction: {TxId}", message.TransactionId);
    }
}