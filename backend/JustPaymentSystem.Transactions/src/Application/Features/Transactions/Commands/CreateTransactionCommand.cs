using Application.Common.Interfaces;
using Domain.Domains;
using Domain.Events;
using FluentValidation;
using Wolverine;

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
    public async Task<TransactionCreated> Handle(
        CreateTransactionCommand command,
        IValidator<CreateTransactionCommand> validator,
        ITransactionRepository transactionRepository,
        IUnitOfWork unitOfWork,
        IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var validation = validator.Validate(command);

        if(!validation.IsValid)
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
        return new TransactionCreated(transaction.Id);
    }
}
