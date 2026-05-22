using Application.Features.Transactions.Queries.DTOs;
using Domain.Domains;

namespace Application.Common.Interfaces.Mappers;

public interface ITransactionMapper
{
    TransactionResponse Map(Transaction transaction);
}
