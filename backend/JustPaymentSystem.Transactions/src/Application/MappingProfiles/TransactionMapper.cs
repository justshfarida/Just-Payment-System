using Application.Common.Interfaces.Mappers;
using Application.Features.Transactions.Queries.DTOs;
using Domain.Domains;

namespace Application.MappingProfiles;

public class TransactionMapper : ITransactionMapper
{
    public TransactionResponse Map(Transaction transaction)
    {
        return new()
        {
            Id = transaction.Id,
            Amount = transaction.Amount,
            FeeAmount = transaction.FeeAmount,
            Currency = transaction.Currency,
            Description = transaction.Description,
            MerchantId = transaction.MerchantId,
            Status = transaction.Status.ToString(),
            CreatedAt = transaction.CreatedAt,
            UpdatedAt = transaction.UpdatedAt
        };
    }
}
