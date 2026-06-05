using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Domains;
using Microsoft.Extensions.Logging;

namespace Application.Features.Transactions.IntegrationEvents;

public record CallbackFailedIntegrationEvent(Guid TransactionId);

public class CallbackFailedIntegrationEventHandler
{
    public static async Task Handle(
        CallbackFailedIntegrationEvent @event,
        ITransactionRepository transactionRepository,
        IUnitOfWork unitOfWork,
        ILogger<CallbackFailedIntegrationEventHandler> logger,
        CancellationToken cancellationToken)
    {
        Transaction? transaction = await transactionRepository.GetByIdAsync(@event.TransactionId, cancellationToken);
        if (transaction == null)
        {
            logger.LogCritical("Transaction from callback-failed event was not found");
            throw new NotFoundException($"Transaction with Id: {@event.TransactionId} was not found");
        }

        // Event handler must call bank-provider to refund money 
        await Task.Delay(2000);

        transaction.Refund();
        await unitOfWork.SaveAsync(cancellationToken);
    }
}