using Domain.Shared.Exceptions;

namespace Domain.Shared.Validations;

public static class DecimalValidation
{
    extension(decimal value)
    {
        public void EnsureGreaterThan(decimal v)
        {
            if (value < v)
            {
                throw new AmountBelowMinimumLimitException($"{value} cannot be less than {v}");
            }
        }

        public void EnsureGreaterThanZero()
        {
            value.EnsureGreaterThan(0);
        }
    }
}
