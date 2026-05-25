namespace Application.Common.Exceptions;

public class IdempotencyKeyDuplicateException : Exception
{
    public IdempotencyKeyDuplicateException()
    {
    }

    public IdempotencyKeyDuplicateException(string? message) : base(message)
    {
    }
}
