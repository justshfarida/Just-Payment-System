using Application.Common.Interfaces;
using Application.Features.Transactions.Queries.DTOs;
using Domain.Shared.Enums;

namespace Application.Features.Transactions.Queries;

public sealed record GetTransactionsQuery(int Page, int PageSize, Guid? MerchantId, string? Currency, TransactionStatus? Status);

public sealed class GetTransactionsQueryHandler
{
    private readonly ITransactionReadRepository _transactionReadRepository;

    public GetTransactionsQueryHandler(ITransactionReadRepository transactionReadRepository)
    {
        _transactionReadRepository = transactionReadRepository;
    }

    public async Task<List<TransactionResponse>> Handle(GetTransactionsQuery request)
    {
        return await _transactionReadRepository.GetAllWithPaginationAsync(request.Page, request.PageSize, request.MerchantId, request.Currency, request.Status);
    }
}
