using Application.Common.Models;
using Application.Features.Transactions.Queries.DTOs;
using Domain.Shared.Enums;

namespace Application.Common.Interfaces;

public interface ITransactionReadRepository
{
    Task<TransactionResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResponse<TransactionResponse>> GetAllWithPaginationAsync(int page, int pageSize, string? merchantId, string? currency, TransactionStatus? status, CancellationToken cancellationToken = default);
}