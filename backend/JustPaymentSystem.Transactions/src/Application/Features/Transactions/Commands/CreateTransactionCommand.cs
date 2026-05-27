using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Services;
using Application.Common.Models;
using Application.Features.Transactions.Commands.DTOs;
using Domain.Domains;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace Application.Features.Transactions.Commands;

// Client request for payment with data and signature
// Transaction creates with pending result 
// Redirect url sent to client
// Client gives card credentials and submits
// Transaction service checks data and signature and updates if payment is succesfull and adds payment snapshot 
// Webhook service send http request to callback endpoint of client with data and signature
// Client callback can send Ok which means state changed and BadRequest for refund
public sealed record CreateTransactionCommand(
    string MerchantId,
    string IdempotencyKey,
    decimal Amount,
    string Currency,
    string OrderId,
    string Description,
    string? SuccessRedirectUrl,
    string? ErrorRedirectUrl,
    string[] OtherAttr
    );

public sealed class CreateTransactionHandler
{
    public async Task<PaymentPageRedirectUrl> Handle(
        CreateTransactionCommand command,
        IValidator<CreateTransactionCommand> validator,
        ITransactionRepository transactionRepository,
        IUnitOfWork unitOfWork,
        ICacheService cacheService,
        IOptions<ClientOptions> clientOp,
        CancellationToken cancellationToken)
    {
        bool transactionExists = await transactionRepository.ExistsAsync(c => c.IdempotencyKey == command.IdempotencyKey);
        if (transactionExists)
        {
            throw new IdempotencyKeyDuplicateException($"IdempotencyKey with {command.IdempotencyKey} already exists");
        }

        // TODO: Validate merchantId

        var validation = validator.Validate(command);

        if (!validation.IsValid)
        {
            throw new ValidationException(validation.Errors);
        }

        Transaction transaction = Transaction.Create(
            command.MerchantId,
            command.IdempotencyKey,
            (long)(command.Amount * 100),
            command.Currency,
            command.Description,
            command.OrderId);

        transaction.AddAttributes(command.OtherAttr);

        await transactionRepository.InsertAsync(transaction, cancellationToken);
        await unitOfWork.SaveAsync(cancellationToken);

        string token = $"txtn_{Guid.NewGuid():N}";

        string successUrl = command.SuccessRedirectUrl ?? $"{clientOp.Value.Url}/"; // Need make it api call to mercahant to get success reidrect url
        string errorUrl = command.ErrorRedirectUrl ?? $"{clientOp.Value.Url}/";

        await cacheService.SetAsync(
            token,
            new TransactionSession(transaction.Id, successUrl, errorUrl, transaction.CreatedAt),
            TimeSpan.FromMinutes(5));

        string redirectUrl = $"{clientOp.Value.Url}/pay/{token}";
        return new PaymentPageRedirectUrl(redirectUrl);
        //return new TransactionCreated(transaction.Id);
    }
}
