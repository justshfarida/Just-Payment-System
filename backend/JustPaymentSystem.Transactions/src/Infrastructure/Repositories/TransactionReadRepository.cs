using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Mappers;
using Application.Common.Models;
using Application.Features.Transactions.Queries.DTOs;
using Domain.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TransactionReadRepository : ITransactionReadRepository
{
    private readonly TransactionDbContext _db;
    private readonly ITransactionMapper _mapper;
    private readonly IPagedResponseFactory _pagedResponseFactory;
    public TransactionReadRepository(TransactionDbContext db, ITransactionMapper mapper, IPagedResponseFactory pagedResponseFactory)
    {
        _db = db;
        _mapper = mapper;
        _pagedResponseFactory = pagedResponseFactory;
    }

    public async Task<PagedResponse<TransactionResponse>> GetAllWithPaginationAsync(int page, int pageSize, string? merchantId, string? currency, TransactionStatus? status, CancellationToken cancellationToken = default)
    {

        var transactions = _db.Transactions
            .Include(c => c.PaymentSnapshot)
            .Where(c => merchantId == null || c.MerchantId == merchantId)
            .Where(c => currency == null || c.Currency == currency)
            .Where(c => status == null || c.Status == status)
            .OrderByDescending(c => c.CreatedAt)
                .ThenByDescending(c => c.UpdatedAt)
            .AsNoTracking()
            .Select(c => _mapper.Map(c));
        var pagedResponse = await _pagedResponseFactory.Create(transactions, page, pageSize);
        return pagedResponse;
    }

    public Task<TransactionResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _db.Transactions
            .Include(c => c.PaymentSnapshot)
            .AsNoTracking()
            .Select(c => _mapper.Map(c))
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }
}