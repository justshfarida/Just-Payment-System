namespace Domain.Shared.Exceptions;

public class InvalidDomainStateException : Exception
{
    public InvalidDomainStateException()
    {
    }

    public InvalidDomainStateException(string? message) : base(message)
    {
    }
}
