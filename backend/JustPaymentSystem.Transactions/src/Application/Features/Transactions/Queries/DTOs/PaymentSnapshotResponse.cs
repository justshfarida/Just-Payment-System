namespace Application.Features.Transactions.Queries.DTOs;

public class PaymentSnapshotResponse
{
    public PaymentSnapshotResponse(Guid id, string type, string maskedIdentifier)
    {
        Id = id;
        Type = type;
        MaskedIdentifier = maskedIdentifier;
    }

    public Guid Id { get; set; }
    public string Type { get; set; } = null!;
    public string MaskedIdentifier { get; set; } = null!;
}
