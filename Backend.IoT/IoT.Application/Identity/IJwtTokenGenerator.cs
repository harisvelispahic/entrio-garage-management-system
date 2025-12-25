namespace IoT.Application.Identity;

public interface IJwtTokenGenerator
{
    string GenerateToken(Guid ownerId, string email);
}
