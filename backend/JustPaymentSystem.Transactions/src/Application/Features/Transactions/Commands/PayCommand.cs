using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Services;
using Application.Features.Transactions.Commands.DTOs;
using Application.Features.Transactions.IntegrationEvents;
using Domain.Domains;
using Domain.Shared.Enums;
using Domain.Shared.Exceptions;
using FluentValidation;

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
        IValidator<PaymentRequest> validator,
        CancellationToken cancellationToken)
    {
        var validation = validator.Validate(command.PaymentRequest);

        if (!validation.IsValid)
        {
            throw new ValidationException(validation.Errors);
        }
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

        if (transaction.Status != TransactionStatus.PENDING)
        {
            throw new InvalidDomainStateException($"Cannot proceed payment from state transaction: {transaction.Status}");
        }

        await Task.Delay(5000);

        await unitOfWork.BeginTransactionAsync();
        try
        {
            transaction.Capture();
            transaction.SetPaymentSnapshot(command.PaymentRequest.CardNumber, PaymentType.CARD);
            await unitOfWork.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }


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
