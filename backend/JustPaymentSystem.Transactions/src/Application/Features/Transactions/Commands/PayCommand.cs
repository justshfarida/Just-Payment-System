using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Services;
using Application.Features.Transactions.Commands.DTOs;
using Application.Features.Transactions.IntegrationEvents;
using Domain.Domains;
using Domain.Shared.Enums;
using Domain.Shared.Exceptions;
using Spectre.Console;

namespace Application.Features.Transactions.Commands;

public record PayCommand(
    string Token,
    PaymentRequest PaymentRequest
    );

public class PayCommandHandler
{
    public async Task<TransactionCompletedIntegrationEvent> Handle(PayCommand command,
        ITransactionRepository transactionRepository,
        ICacheService cacheService,
        IUnitOfWork unitOfWork,
        CancellationToken cancellationToken)
    {
        TransactionSession? session = await cacheService.GetAsync<TransactionSession>(command.Token);
        if (session is null)
        {
            throw new NotFoundException();
        }

        if (session.CreatedAt.AddMinutes(5) < DateTime.UtcNow)
        {
            throw new TransactionSessionExpiredException();
        }

        Transaction? transaction = await transactionRepository.GetByIdAsync(session.TransactionId, cancellationToken);

        if (transaction is null)
        {
            throw new NotFoundException(
               $"Transaction with id '{session.TransactionId}' was not found.");
        }

        if(transaction.Status != TransactionStatus.PENDING)
        {
            throw new InvalidDomainStateException($"Cannot proceed payment from state transaction: {transaction.Status}");
        }

        transaction.SetPaymentSnapshot(command.PaymentRequest.CardNumber, PaymentType.CARD);

        // Here command makes request to bank
        //await Task.Delay(5000);

        transaction.Capture();

        await unitOfWork.SaveAsync(cancellationToken);

        return new TransactionCompletedIntegrationEvent(
            transaction.Id,  
            transaction.MerchantId,
            transaction.OrderId,
            transaction.Amount,
            transaction.Currency,
            session.SuccessUrl,
            session.ErrorUrl,
            DateTime.UtcNow
        );
    }
}
