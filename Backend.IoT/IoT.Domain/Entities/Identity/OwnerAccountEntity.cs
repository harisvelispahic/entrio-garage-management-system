namespace IoT.Domain.Entities.Identity;

public class OwnerAccountEntity
{
    public Guid Id { get; private set; }

    public string Email { get; private set; } = null!;
    public string PinHash { get; private set; } = null!;
    public string PinSalt { get; private set; } = null!;

    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? LastLoginAtUtc { get; private set; }

    private OwnerAccountEntity() { }

    public OwnerAccountEntity(string email, string pinHash, string pinSalt)
    {
        Id = Guid.NewGuid();
        Email = email;
        PinHash = pinHash;
        PinSalt = pinSalt;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void MarkLogin()
    {
        LastLoginAtUtc = DateTime.UtcNow;
    }
}
