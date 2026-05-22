namespace Domain.Shared.Exceptions;

public class AmountBelowMinimumLimitException : Exception
{
    public AmountBelowMinimumLimitException()
    {
    }

    public AmountBelowMinimumLimitException(string? message) : base(message)
    {
    }
}
