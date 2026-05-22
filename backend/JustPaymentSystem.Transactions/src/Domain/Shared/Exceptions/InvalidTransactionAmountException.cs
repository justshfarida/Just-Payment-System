namespace Domain.Shared.Exceptions;

public class InvalidTransactionAmountException : Exception
{
    public InvalidTransactionAmountException(long amount, long minimumRequired, string currency)
    {
        Amount = amount;
        MinimumRequired = minimumRequired;
        Currency = currency;
    }

    public long Amount { get; set; }
    public long MinimumRequired { get; set; }
    public string Currency { get; set; }
    
}
