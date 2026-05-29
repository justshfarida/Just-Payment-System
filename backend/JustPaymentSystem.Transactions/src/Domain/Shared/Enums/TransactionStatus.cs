namespace Domain.Shared.Enums;

public enum TransactionStatus : byte
{
    PENDING = 1,
    AUTHORIZED,
    CAPTURED,
    FAILED,
    VOIDED,
    REFUNDED
}
