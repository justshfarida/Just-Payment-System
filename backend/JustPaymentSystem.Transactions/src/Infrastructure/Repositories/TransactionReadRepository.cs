using Application.Common.Interfaces;
using Application.Common.Interfaces.Mappers;
using Application.Features.Transactions.Queries.DTOs;
using Domain.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TransactionReadRepository : ITransactionReadRepository
{
    private readonly TransactionDbContext _db;
    private readonly ITransactionMapper _mapper;
    public TransactionReadRepository(TransactionDbContext db, ITransactionMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public Task<List<TransactionResponse>> GetAllWithPaginationAsync(int page, int pageSize, string? merchantId, string? currency, TransactionStatus? status, CancellationToken cancellationToken = default)
    {
        return _db.Transactions
            .Include(c => c.PaymentSnapshot)
            .Where(c => merchantId == null || c.MerchantId == merchantId)
            .Where(c => currency == null || c.Currency == currency)
            .Where(c => status == null || c.Status == status)
            .OrderByDescending(c => c.UpdatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .Select(c => _mapper.Map(c))
            .ToListAsync(cancellationToken);
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