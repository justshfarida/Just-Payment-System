using Application.Common.Interfaces;
using Application.Features.Transactions.Queries.DTOs;
using Domain.Shared.Enums;

namespace Application.Features.Transactions.Queries;

public sealed record GetTransactionsQuery(int Page, int PageSize, string? MerchantId, string? Currency, TransactionStatus? Status);

public sealed class GetTransactionsQueryHandler
{

    public async Task<List<TransactionResponse>> Handle(GetTransactionsQuery request, ITransactionReadRepository transactionReadRepository)
    {
        return await transactionReadRepository.GetAllWithPaginationAsync(request.Page, request.PageSize, request.MerchantId, request.Currency, request.Status);
    }
}
