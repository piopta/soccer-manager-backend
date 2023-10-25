using Microsoft.AspNetCore.Identity;
using System.Net.Http.Json;
using WebApi.Tests.DataGenerators;

namespace WebApi.Tests;

public class AuthEndpointsTests : IClassFixture<CustomApiFactory>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthEndpointsTests(CustomApiFactory factory)
    {
        _client = factory.CreateClient();
        _userManager = factory.UserManager;
    }

    public async Task DisposeAsync()
    {
        IList<ApplicationUser> users = await _userManager.GetUsersInRoleAsync("User");

        foreach (var u in users)
        {
            await _userManager.DeleteAsync(u);
        }
    }

    public async Task InitializeAsync()
    {
        RegisterUser validUser = new()
        {
            Email = "tess.stokes12@ethereal.email",
            Password = "myPassword123!",
            ConfirmPassword = "myPassword123!",
        };

        RegisterUser invalidUser = new()
        {
            Email = "tess.stokes13@ethereal.email",
            Password = "myPassword123!",
            ConfirmPassword = "myPassword123!",
        };

        await CreateUserInAppAsync(validUser, true);
        await CreateUserInAppAsync(invalidUser, false);
    }

    [Theory]
    [ClassData(typeof(RegisterUserGenerator))]
    public async Task RegisterUser_ShouldReturnStatusCode(RegisterUser user, HttpStatusCode code)
    {
        var res = await _client.PostAsJsonAsync("/api/auth/register", user);

        Assert.Equal(code, res.StatusCode);
    }

    [Theory]
    [ClassData(typeof(LoginUserGenerator))]
    public async Task LoginUser_ShouldReturnStatusCode(LoginUser user, HttpStatusCode code)
    {
        var res = await _client.PostAsJsonAsync("/api/auth/login", user);

        Assert.Equal(code, res.StatusCode);
    }

    private async Task CreateUserInAppAsync(RegisterUser user, bool confirmed)
    {
        ApplicationUser appUser = new()
        {
            UserName = user.Email,
            Email = user.Email,
            EmailConfirmed = confirmed
        };

        await _userManager.CreateAsync(appUser, user.Password);

        await _userManager.AddToRoleAsync(appUser, ApplicationConstants.Roles.User);
    }
}
