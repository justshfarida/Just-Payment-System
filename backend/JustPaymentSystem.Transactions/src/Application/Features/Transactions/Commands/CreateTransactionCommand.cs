using Application.Common.Interfaces;
using Application.Features.Transactions.Events;
using Domain.Domains;
using Domain.Shared.Enums;

namespace Application.Features.Transactions.Commands;

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

public sealed class CreateTransactionCommandHandler
{
    public async Task<TransactionCreated> Handle(
        CreateTransactionCommand command,
        ITransactionRepository transactionRepository,
        IUnitOfWork unitOfWork,
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
        return new(transaction.Id);
    }
}
