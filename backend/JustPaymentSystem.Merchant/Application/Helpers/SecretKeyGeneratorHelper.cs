using System.Security.Cryptography;

namespace Application.Helpers;

public static class SecretKeyGeneratorHelper
{
    public static string GenerateSecretKey()
    {
        return GenerateHexKey(32);
    }

    public static string GenerateBase64Key(int bytesLength)
    {
        byte[] randomBytes = new byte[bytesLength];

        // Fills the array with a cryptographically strong sequence of random values
        RandomNumberGenerator.Fill(randomBytes);

        return Convert.ToBase64String(randomBytes);
    }

    /// <summary>
    /// Generates a cryptographically secure random key formatted as a Hexadecimal string.
    /// </summary>
    public static string GenerateHexKey(int bytesLength)
    {
        byte[] randomBytes = new byte[bytesLength];

        RandomNumberGenerator.Fill(randomBytes);

        return Convert.ToHexString(randomBytes);
    }
}
