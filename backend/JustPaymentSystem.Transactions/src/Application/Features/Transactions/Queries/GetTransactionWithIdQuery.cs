using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Features.Transactions.Queries.DTOs;

namespace Application.Features.Transactions.Queries;

public sealed record GetTransactionByIdQuery(Guid Id);

public sealed class GetTransactionByIdHandler
{
    private readonly ITransactionReadRepository _transactionReadRepository;

    public GetTransactionByIdHandler(
        ITransactionReadRepository transactionReadRepository)
    {
        _transactionReadRepository = transactionReadRepository;
    }

    public async Task<TransactionResponse> Handle(
        GetTransactionByIdQuery query)
    {
        var transaction = await _transactionReadRepository
            .GetByIdAsync(query.Id);

        if (transaction is null)
        {
            throw new NotFoundException(
                $"Transaction with id '{query.Id}' was not found.");
        }

        return transaction;
    }
}