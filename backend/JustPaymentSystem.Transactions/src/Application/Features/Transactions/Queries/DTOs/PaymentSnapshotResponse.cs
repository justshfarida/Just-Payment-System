namespace Application.Features.Transactions.Queries.DTOs;

public class PaymentSnapshotResponse
{
    public PaymentSnapshotResponse(Guid id, string type, string maskedIdentifier)
    {
        Type = type;
        MaskedIdentifier = maskedIdentifier;
        Id = id;
    }

    public Guid Id { get; set; }
    public string Type { get; set; } = null!;
    public string MaskedIdentifier { get; set; } = null!;
}
