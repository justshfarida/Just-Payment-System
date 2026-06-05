using Application.Common.Interfaces;
using Application.Features.Transactions.Queries.DTOs;

namespace Application.Features.Transactions.Queries;

public record GetTransactionStatsQuery(string MerchantId);

public class GetTransactionStatsQueryHandler
{

    public async Task<TransactionStatsResponse> Handle(GetTransactionStatsQuery query, ITransactionRepository transactionRepostiory, CancellationToken cancellationToken)
    {
        var transactions = await transactionRepostiory.GetAllAsync(1, 50, query.MerchantId, null, cancellationToken);
        int total = await transactionRepostiory.CountAsync(c => true);
        int successCount = transactions.Count(c => c.Status == Domain.Shared.Enums.TransactionStatus.CAPTURED);
        int failCount = transactions.Count(c => c.Status != Domain.Shared.Enums.TransactionStatus.CAPTURED);
        long revenue = transactions.Where(c => c.Status == Domain.Shared.Enums.TransactionStatus.CAPTURED).Sum(c => c.Amount);

        return new(total, successCount, failCount, revenue);
    }
}