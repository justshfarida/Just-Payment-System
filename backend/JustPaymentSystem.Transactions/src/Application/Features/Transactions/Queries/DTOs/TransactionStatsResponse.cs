using Spectre.Console;

namespace Application.Features.Transactions.Queries.DTOs;

public record TransactionStatsResponse(int TotalCount, int SucceedCount, int FailedCount, long Revenue);
