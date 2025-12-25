using IoT.Application.Identity;
using System.Security.Cryptography;
using System.Text;

namespace IoT.Infrastructure.Identity;

public class PinHasher : IPinHasher
{
    public (string hash, string salt) Hash(string pin)
    {
        var saltBytes = RandomNumberGenerator.GetBytes(16);
        var pinBytes = Encoding.UTF8.GetBytes(pin);

        var combined = new byte[saltBytes.Length + pinBytes.Length];
        Buffer.BlockCopy(saltBytes, 0, combined, 0, saltBytes.Length);
        Buffer.BlockCopy(pinBytes, 0, combined, saltBytes.Length, pinBytes.Length);

        var hash = SHA256.HashData(combined);

        return (Convert.ToBase64String(hash), Convert.ToBase64String(saltBytes));
    }

    public bool Verify(string pin, string hash, string salt)
    {
        var saltBytes = Convert.FromBase64String(salt);
        var pinBytes = Encoding.UTF8.GetBytes(pin);

        var combined = new byte[saltBytes.Length + pinBytes.Length];
        Buffer.BlockCopy(saltBytes, 0, combined, 0, saltBytes.Length);
        Buffer.BlockCopy(pinBytes, 0, combined, saltBytes.Length, pinBytes.Length);

        var computedHash = SHA256.HashData(combined);
        return Convert.ToBase64String(computedHash) == hash;
    }
}
