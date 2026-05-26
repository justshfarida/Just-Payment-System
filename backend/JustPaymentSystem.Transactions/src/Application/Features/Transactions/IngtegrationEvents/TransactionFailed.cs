using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Features.Transactions.IngtegrationEvents;

public record TransactionFailed(Guid TransactionId);

public class TransactionFailedHandler
{
    public async Task Handle(TransactionFailed @event,
        ITransactionRepository transactionRepository,
        IUnitOfWork unitOfWork,
        ILogger<TransactionFailedHandler> logger,
        CancellationToken cancellationToken)
    {
        var transaction = await transactionRepository.GetByIdAsync(@event.TransactionId, cancellationToken);
        if (transaction == null)
        {
            logger.LogCritical("Invalid failed transaction with Id: {TransactionId}", @event.TransactionId);
            throw new NotFoundException();
        }
        transaction.TransactionFailed();
        await unitOfWork.SaveAsync(cancellationToken);
    }
}