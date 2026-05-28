using Microsoft.Extensions.Logging;
using Wolverine;

namespace Application.Features.Transactions.IntegrationEvents;

public record TransactionCompletedIntegrationEvent(
    Guid TransactionId,
    string MerchantId,
    string OrderId,
    decimal Amount,
    string Currency,
    string SuccessRedirectUrl,
    string ErrorRedirectUrl,
    DateTime Timestamp
);

public class TransactionCompletedIntegrationEventHandler
{
    public static async Task Handle(TransactionCompletedIntegrationEvent @event, IMessageBus bus, ILogger<TransactionCompletedIntegrationEventHandler> logger)
    {
        logger.LogInformation("Event with transactionId: {transactionId} is publishing", @event.TransactionId);
        await bus.PublishAsync(@event);
    }
}