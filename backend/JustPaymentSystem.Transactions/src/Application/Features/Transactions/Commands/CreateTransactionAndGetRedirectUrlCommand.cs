using Application.Common.Interfaces;
using Application.Features.Transactions.Events;
using Domain.Domains;
using Domain.Events;
using Wolverine;

namespace Application.Features.Transactions.Commands;

// Client request for payment with data and signature
// Transaction creates with pending result 
// Redirect url sent to client
// Client gives card credentials and submits
// Transaction service checks data and signature and updates if payment is succesfull and adds payment snapshot 
// Webhook service send http request to callback endpoint of client with data and signature
// Client callback can send Ok which means state changed and BadRequest for refund
public sealed record CreateTransactionAndGetRedirectUrlCommand(
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

public sealed class CreateTransactionAndGetRedirectUrlHandler
{
    public async Task Handle(
        CreateTransactionAndGetRedirectUrlCommand command,
        ITransactionRepository transactionRepository,
        IUnitOfWork unitOfWork,
        IMessageBus bus,
        CancellationToken cancellationToken)
    {
        // TODO: Need add validation !!!

        Transaction transaction = Transaction.Create(
            Guid.Parse(command.MerchantId),
            command.IdempotencyKey,
            (long)(command.Amount * 100),
            command.Currency,
            command.Description,
            command.OrderId);
        transaction.AddAttributes(command.OtherAttr);
        await transactionRepository.InsertAsync(transaction, cancellationToken);
        await unitOfWork.SaveAsync(cancellationToken);

    }
}
