using Api.DTOs;
using Api.Events;

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
        _logger.LogInformation("Processing success webhook for Transaction: {TxId}", message.TransactionId);

        // 1. Define the event type we are looking for based on what happened
        string eventTypeName = "payment.success";

        // 2. DYNAMIC CALL: Ask the Merchant Service for this merchant's endpoint configuration
        // We pass the MerchantId from the message and the event type we want to trigger
        var merchantSettingsUrl = $"http://merchant-service-internal/api/merchants/{message.MerchantId}/webhooks?eventType={eventTypeName}";

        var settingsResponse = await _httpClient.GetFromJsonAsync<MerchantWebhookSettingsDto>(merchantSettingsUrl);

        // If the merchant hasn't set up a webhook, or turned it off (IsActive == false), stop here safely
        if (settingsResponse == null || string.IsNullOrEmpty(settingsResponse.WebhookUrl))
        {
            _logger.LogWarning("No active webhook configured for Merchant: {MerchantId} for event {Event}", message.MerchantId, eventTypeName);
            return;
        }

        // 3. EXTRACT DATABASE VALUES: Now we are using the real URL from the DB!
        string realMerchantWebhookUrl = settingsResponse.WebhookUrl;
        string secretKey = settingsResponse.SecretKey; // Used for signing later

        // 4. Construct the official JSON body using the verified event type name
        var webhookPayload = new
        {
            @event = eventTypeName, // Dynamically driven by our system rules
            transactionId = message.TransactionId,
            orderId = message.OrderId,
            amount = message.Amount,
            currency = message.Currency,
            timestamp = message.Timestamp
        };

        // 5. SEND TO THE REAL DESTINATION: No longer mock-merchant-shop.com!
        var response = await _httpClient.PostAsJsonAsync(realMerchantWebhookUrl, webhookPayload);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Merchant endpoint {realMerchantWebhookUrl} returned status code: {response.StatusCode}");
        }

        _logger.LogInformation("Successfully delivered webhook to {Url} for Transaction: {TxId}", realMerchantWebhookUrl, message.TransactionId);
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