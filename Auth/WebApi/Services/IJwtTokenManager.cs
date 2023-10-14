namespace WebApi.Services;

public interface IJwtTokenManager
{
    Task InvalidateTokenForUser(string userName, string token);
}