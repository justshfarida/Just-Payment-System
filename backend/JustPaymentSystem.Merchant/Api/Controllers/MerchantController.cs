using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MerchantsController : ControllerBase
{
    private readonly IMerchantService _merchantService;

    public MerchantsController(IMerchantService merchantService)
    {
        _merchantService = merchantService;
    }

    [HttpGet("{id:guid}/webhooks")]
    public async Task<IActionResult> GetWebhookSettings(Guid id, [FromQuery] string eventType)
    {
        var settings = await _merchantService.GetMerchantWebhookSettingsAsync(id, eventType);

        if (settings == null)
        {
            return NotFound(new { message = "Active webhook endpoint or event subscription not found." });
        }

        return Ok(settings);
    }

    [HttpPost("seed-test-data")]
    public async Task<IActionResult> SeedTestData([FromServices] Infrastructure.Persistence.MerchantDbContext context)
    {
        if (context.Merchants.Any())
        {
            return BadRequest(new { message = "Database already contains seed data." });
        }

        // 1. Create a valid Business Type entry
        var retailBusinessType = new Domain.Entitites.BusinessType
        {
            Id = Guid.NewGuid(),
            Name = "E-Commerce & Retail",
        };

        // 2. Create a target Event Type
        var paymentSuccessEvent = new Domain.Entitites.EventType
        {
            Id = Guid.NewGuid(),
            Name = "payment.success",
        };

        // 3. Instantiate the Merchant matching your precise schema fields
        var testMerchant = new Domain.Entitites.Merchant
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Name = "Acme Global Marketplace",
            VOEN = "1234567890",
            Status = Domain.Enums.Status.Active, // Uses your Domain.Enums namespace
            UserId = Guid.NewGuid(),             // Satisfies the required Guid parameter
            Type = retailBusinessType            // Sets the navigation object property directly
        };

        // 4. Attach Webhook Configurations
        var webhookConfig = new Domain.Entitites.Webhook
        {
            Id = Guid.NewGuid(),
            MerchantId = testMerchant.Id,
            WebhookUrl = "https://webhook.site/dummy-endpoint-url",
            SecretKey = "whsec_JustPaymentSecretKeyTesting123!",
            IsActive = true
        };

        // 5. Connect the relationship link assignment
        var subscription = new Domain.Entitites.WebhookEvent
        {
            WebhookId = webhookConfig.Id,
            EventId = paymentSuccessEvent.Id,
            Webhook = webhookConfig,
            Event = paymentSuccessEvent
        };

        // Stage everything into the tracking graph context sequentially
        await context.BusinessTypes.AddAsync(retailBusinessType);
        await context.EventTypes.AddAsync(paymentSuccessEvent);
        await context.Merchants.AddAsync(testMerchant);
        await context.Webhooks.AddAsync(webhookConfig);
        await context.WebhookEvents.AddAsync(subscription);

        await context.SaveChangesAsync();

        return Ok(new
        {
            message = "Test data seeded successfully!",
            targetMerchantId = testMerchant.Id,
            validEventType = "payment.success"
        });
    }
}