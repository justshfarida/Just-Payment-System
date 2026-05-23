using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Features.Transactions.Queries.DTOs;

namespace Application.Features.Transactions.Queries;

public sealed record GetTransactionByIdQuery(Guid Id);

public sealed class GetTransactionByIdHandler
{

    public async Task<TransactionResponse> Handle(
        GetTransactionByIdQuery query,
        ITransactionReadRepository transactionReadRepository)
    {
        var transaction = await transactionReadRepository
            .GetByIdAsync(query.Id);

        if (transaction is null)
        {
            throw new NotFoundException(
                $"Transaction with id '{query.Id}' was not found.");
        }

        return transaction;
    }
}