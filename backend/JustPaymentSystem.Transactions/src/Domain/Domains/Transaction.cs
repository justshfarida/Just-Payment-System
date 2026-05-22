using Domain.Shared.Validations;
using Domain.Shared.Enums;
using Domain.Shared.Exceptions;

namespace Domain.Domains;

public class Transaction : Entity<Guid>
{
    internal Transaction() { }

    private Transaction(
        Guid merchantId,
        long amount,
        string currency,
        long feeAmount,
        TransactionStatus status,
        string description,
        PaymentSnapshot paymentSnapshot)
    {
        Id = Guid.NewGuid(); 
        MerchantId = merchantId;
        Amount = amount;
        Currency = currency;
        FeeAmount = feeAmount;
        Status = status;
        Description = description;
        PaymentSnapshot = paymentSnapshot;
    }

    public Guid MerchantId { get; private set; }
    public long Amount { get; private set; }
    public string Currency { get; private set; } = null!;
    public long FeeAmount { get; private set; }
    public TransactionStatus Status { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public Guid PaymentSnapshotId { get; init; }
    public PaymentSnapshot PaymentSnapshot { get; private set; }

    public void SetStatus(TransactionStatus status)
    {
        this.Status = status;
    }

    public static Transaction Create(Guid merchantId, long amount, string currency, string description, PaymentType paymentType, string card)
    {
        currency.EnsureNotNullOrEmpty();
        description.EnsureNotNull();

        if (amount <= 0)
        {
            throw new InvalidTransactionAmountException(amount, 1, currency);
        }

        long feeAmount = (long)Math.Round((double)amount * 0.03, MidpointRounding.AwayFromZero);

        return new Transaction(
            merchantId,
            amount,
            currency,
            feeAmount,
            TransactionStatus.PENDING,
            description,
            PaymentSnapshot.Create(paymentType, card));
    }

    public void Authorize()
    {
        if (Status != TransactionStatus.PENDING)
            throw new InvalidDomainStateException($"Cannot authorize transaction from state: {Status}");

        Status = TransactionStatus.AUTHORIZED;
    }
}
