using Microsoft.AspNetCore.Mvc;
using OneOf.Types;

namespace WebApi.Services;

public class AppUserFilterService : IAppUserFilterService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AppUserFilterService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<OneOf<BadRequestObjectResult, True>> CheckUserStateAsync(string? email)
    {
        ApplicationUser? appUser = await _userManager.FindByEmailAsync(email);

        if (appUser is null)
        {
            return new BadRequestObjectResult("User account is locked or doesn't exist.");
        }

        if (!appUser.EmailConfirmed)
        {
            return new BadRequestObjectResult("User account is locked or doesn't exist.");
        }

        return new True();
    }
}
