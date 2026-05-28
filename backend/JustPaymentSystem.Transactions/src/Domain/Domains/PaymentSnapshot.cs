using Domain.Shared.Enums;
using Domain.Shared.Validations;

namespace Domain.Domains;

public class PaymentSnapshot : Entity<Guid>
{
    private PaymentSnapshot(Guid transactionId, PaymentType type, string maskedIdentifier)
    {
        Id = Guid.NewGuid();
        TransactionId = transactionId;
        Type = type;
        MaskedIdentifier = maskedIdentifier;
    }

    internal PaymentSnapshot() { }

    public Guid TransactionId { get; init; }
    public Transaction Transaction { get; set; }
    public PaymentType Type { get; private set; }
    public string MaskedIdentifier { get; private set; } = null!;

    public void SetMaskedIdentifier(string value)
    {
        value.EnsureNotNullOrEmpty();

        string maskedValue = Type switch
        {
            PaymentType.CARD => MaskCardNumber(value),
            PaymentType.BANK_TRANSFER => MaskBankIdentifier(value),
            _ => MaskGenericToken(value)
        };
    }

    public static PaymentSnapshot Create(Guid transactionId, PaymentType paymentType, string sensitiveIdentifier)
    {
        sensitiveIdentifier.EnsureNotNullOrEmpty();

        string maskedValue = paymentType switch
        {
            PaymentType.CARD => MaskCardNumber(sensitiveIdentifier),
            PaymentType.BANK_TRANSFER => MaskBankIdentifier(sensitiveIdentifier),
            _ => MaskGenericToken(sensitiveIdentifier)
        };

        return new PaymentSnapshot(transactionId, paymentType, maskedValue);
    }

    private static string MaskCardNumber(string card)
    {
        if (card.Length < 12)
        {
            return MaskGenericToken(card);
        }

        return $"{card[..4]}****";
    }

    private static string MaskBankIdentifier(string iban)
    {
        if (iban.Length < 8) return "****";
        return $"{iban[..4]}********{iban[^4..]}";
    }

    private static string MaskGenericToken(string token)
    {
        if (token.Length <= 4) return "****";
        return $"***_{token[^4..]}";
    }
}