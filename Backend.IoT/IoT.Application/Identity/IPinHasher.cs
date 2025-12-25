namespace IoT.Application.Identity;

public interface IPinHasher
{
    (string hash, string salt) Hash(string pin);
    bool Verify(string pin, string hash, string salt);
}
