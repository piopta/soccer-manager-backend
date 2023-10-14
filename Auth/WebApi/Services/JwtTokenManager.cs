using Microsoft.EntityFrameworkCore;
using OneOf.Types;

namespace WebApi.Services;

public class JwtTokenManager : IJwtTokenManager
{
    private readonly ApplicationDbContext _ctx;
    private readonly ILogger<JwtTokenManager> _logger;

    public JwtTokenManager(ApplicationDbContext ctx, ILogger<JwtTokenManager> logger)
    {
        _ctx = ctx;
        _logger = logger;
    }

    public async Task InvalidateTokenForUser(string userName, string token)
    {
        var user = await GetUserAsync(userName);

        await user.Match(
           async (u) =>
           {
               string tokenHash = token.CreateMD5Hash();

               InvalidToken invalidToken = new()
               {
                   TokenHash = tokenHash,
                   User = u,
                   UserId = u.Id
               };

               u.InvalidTokens.Add(invalidToken);

               await _ctx.SaveChangesAsync();

               return;
           },
           (f) =>
           {
               _logger.LogInformation("User with {userName} not found", userName);
               return Task.CompletedTask;
           }
        );
    }

    private async Task<OneOf<ApplicationUser, False>> GetUserAsync(string userName)
    {
        ApplicationUser? user = await _ctx.Users.FirstOrDefaultAsync(u => u.UserName == userName);

        if (user is null)
        {
            return new False();
        }

        return user;
    }
}
