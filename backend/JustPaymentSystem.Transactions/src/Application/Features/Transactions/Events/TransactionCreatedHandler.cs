using Microsoft.Extensions.Logging;
using Domain.Events;
namespace Application.Features.Transactions.Events;

public class TransactionCreatedHandler
{
    public Task Handle(TransactionCreated transactionCreated, ILogger<TransactionCreatedHandler> logger)
    {
        logger.LogInformation("Transacion was created {transactionId}", transactionCreated.TransactionId);
        return Task.CompletedTask;
    }
}