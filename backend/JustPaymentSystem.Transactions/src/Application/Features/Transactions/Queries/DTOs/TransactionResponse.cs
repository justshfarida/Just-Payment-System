namespace Application.Features.Transactions.Queries.DTOs;

public class TransactionResponse
{
    public Guid Id { get; set; }
    public string MerchantId { get; set; }
    public long Amount { get; set; }
    public string Currency { get; set; } = null!;
    public long FeeAmount { get; set; }
    public string Status { get; set; }
    public string Description { get; set; } = string.Empty;
    public PaymentSnapshotResponse? PaymentSnapshot { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
