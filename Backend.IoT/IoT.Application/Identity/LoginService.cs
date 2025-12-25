using IoT.Application.Common;
using Microsoft.EntityFrameworkCore;

namespace IoT.Application.Identity;

public class LoginService
{
    private readonly IAppDbContext _db;
    private readonly IPinHasher _pinHasher;
    private readonly IJwtTokenGenerator _jwt;

    public LoginService(
        IAppDbContext db,
        IPinHasher pinHasher,
        IJwtTokenGenerator jwt)
    {
        _db = db;
        _pinHasher = pinHasher;
        _jwt = jwt;
    }

    public async Task<string> LoginAsync(LoginRequest request)
    {
        var owner = await _db.OwnerAccounts.SingleOrDefaultAsync();

        if (owner is null)
            throw new Exception("Owner account not initialized.");

        if (!owner.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase))
            throw new Exception("Invalid credentials.");

        if (!_pinHasher.Verify(request.Pin, owner.PinHash, owner.PinSalt))
            throw new Exception("Invalid credentials.");

        owner.MarkLogin();
        await _db.SaveChangesAsync();

        return _jwt.GenerateToken(owner.Id, owner.Email);
    }
}
