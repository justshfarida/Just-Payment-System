namespace Domain.Shared.Validations;

public static class StringValidation
{
    extension(string value)
    {
        public void EnsureNotNull()
        {
            if(value == null)
            {
                throw new ArgumentNullException();
            }
        }

        public void EnsureNotNullOrEmpty()
        {
            if(string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException();
            }
        }
    }
}
