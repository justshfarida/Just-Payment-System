using Domain.Events;
using Domain.Shared.Enums;
using Domain.Shared.Exceptions;
using Domain.Shared.Validations;

namespace Domain.Domains;

public class Transaction : AggregateRoot<Guid>
{
    private readonly List<TransactionAttribute> _attributes = new();
    internal Transaction() { }

    private Transaction(
        string merchantId,
        long amount,
        string currency,
        long feeAmount,
        TransactionStatus status,
        string description,
        PaymentSnapshot? paymentSnapshot,
        string orderId,
        string ıdempotencyKey)
    {
        Id = Guid.NewGuid();
        MerchantId = merchantId;
        Amount = amount;
        Currency = currency;
        FeeAmount = feeAmount;
        Status = status;
        Description = description;
        PaymentSnapshot = paymentSnapshot;
        OrderId = orderId;
        IdempotencyKey = ıdempotencyKey;
    }

    public string MerchantId { get; private set; }
    public string IdempotencyKey { get; set; } = null!;
    public long Amount { get; private set; }
    public string Currency { get; private set; } = null!;
    public long FeeAmount { get; private set; }
    public TransactionStatus Status { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public string OrderId { get; set; } = null!;
    public Guid? PaymentSnapshotId { get; init; }
    public PaymentSnapshot? PaymentSnapshot { get; private set; }
    public IReadOnlyList<TransactionAttribute> Attributes => _attributes.AsReadOnly();

    public void Authorize()
    {
        if (Status != TransactionStatus.PENDING)
            throw new InvalidDomainStateException($"Cannot authorize transaction from state: {Status}");

        Status = TransactionStatus.AUTHORIZED;
    }

    public void Capture()
    {
        if (Status != TransactionStatus.PENDING && Status != TransactionStatus.AUTHORIZED)
            throw new InvalidDomainStateException($"Cannot authorize transaction from state: {Status}");

        Status = TransactionStatus.CAPTURED;
    }

    public void TransactionFailed()
    {
        if (Status != TransactionStatus.PENDING && Status != TransactionStatus.AUTHORIZED)
            throw new InvalidDomainStateException($"Cannot authorize transaction from state: {Status}");

        Status = TransactionStatus.FAILED;
    }

    public void TransactionVoided()
    {
        if (Status != TransactionStatus.PENDING && Status != TransactionStatus.AUTHORIZED)
            throw new InvalidDomainStateException($"Cannot authorize transaction from state: {Status}");

        Status = TransactionStatus.VOIDED;
    }
    public void Refund()
    {
        if (Status != TransactionStatus.CAPTURED)
            throw new InvalidDomainStateException($"Cannot refund transaction from state: {Status}");

        Status = TransactionStatus.REFUNDED;
    }

    public void AddAttribute(string attribute)
    {
        _attributes.Add(TransactionAttribute.Create(attribute));
    }

    public void AddAttributes(string[] attributes)
    {
        _attributes.AddRange(attributes.Select(TransactionAttribute.Create));
    }

    public void RemoveAttribute(Guid attributeId)
    {
        var attribute = _attributes.First(c => c.Id == attributeId);
        _attributes.Remove(attribute);
    }

    public void SetPaymentSnapshot(string card, PaymentType paymentType)
    {
        PaymentSnapshot = PaymentSnapshot.Create(Id, paymentType, card);
    }

    public static Transaction Create(string merchantId, string idempotencyKey, long amount, string currency, string description, string orderId)
    {
        currency.EnsureNotNullOrEmpty();
        description.EnsureNotNull();
        idempotencyKey.EnsureNotNullOrEmpty();

        if (amount <= 0)
        {
            throw new InvalidTransactionAmountException(amount, 1, currency);
        }

        long feeAmount = (long)Math.Round((double)amount * 0.03, MidpointRounding.AwayFromZero);

        var transaction = new Transaction(
            merchantId,
            amount,
            currency,
            feeAmount,
            TransactionStatus.PENDING,
            description,
            null,
            orderId,
            idempotencyKey);

        transaction.RaiseDomainEvent(new TransactionCreated(transaction.Id));
        return transaction;
    }
    public static Transaction Create(string merchantId, string idempotencyKey, long amount, string currency, string description, PaymentType paymentType, string card, string orderId)
    {
        currency.EnsureNotNullOrEmpty();
        description.EnsureNotNull();
        idempotencyKey.EnsureNotNullOrEmpty();

        if (amount <= 0)
        {
            throw new InvalidTransactionAmountException(amount, 1, currency);
        }

        long feeAmount = (long)Math.Round((double)amount * 0.03, MidpointRounding.AwayFromZero);

        var transaction = new Transaction(
            merchantId,
            amount,
            currency,
            feeAmount,
            TransactionStatus.PENDING,
            description,
            null,
            orderId,
            idempotencyKey);

        transaction.SetPaymentSnapshot(card, paymentType);

        transaction.RaiseDomainEvent(new TransactionCreated(transaction.Id));
        return transaction;
    }

}
