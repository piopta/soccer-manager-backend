using Microsoft.AspNetCore.Identity;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using WebApi.Data;
using WebApi.Tests.DataGenerators;

namespace WebApi.Tests;

public class AuthEndpointsTests : IClassFixture<CustomApiFactory>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ApplicationDbContext _ctx;
    private readonly Func<Task> _resetDatabaseState;

    public AuthEndpointsTests(CustomApiFactory factory)
    {
        _client = factory.CreateClient();
        _userManager = factory.UserManager;
        _roleManager = factory.RoleManager;
        _ctx = factory.DbContext;
        _resetDatabaseState = factory.ResetDatabaseState;
    }

    public async Task DisposeAsync()
    {
        await _resetDatabaseState();
    }

    public async Task InitializeAsync()
    {
        await _roleManager.CreateAsync(new(ApplicationConstants.Roles.User));
        await _roleManager.CreateAsync(new(ApplicationConstants.Roles.Admin));

        await CreateUserInAppAsync(TestConstants.validUser, true);
        await CreateUserInAppAsync(TestConstants.validUser2, true);
        await CreateUserInAppAsync(TestConstants.invalidUser, false);
    }

    [Theory]
    [ClassData(typeof(RegisterUserGenerator))]
    public async Task RegisterUser_ShouldReturnStatusCode(RegisterUser user, HttpStatusCode code)
    {
        await PerformCallAsync("/api/auth/register", user, code);
    }

    [Theory]
    [ClassData(typeof(LoginUserGenerator))]
    public async Task LoginUser_ShouldReturnStatusCode(LoginUser user, HttpStatusCode code)
    {
        await PerformCallAsync("/api/auth/login", user, code);
    }

    [Theory]
    [ClassData(typeof(ForgotPasswordUserGenerator))]
    public async Task ForgotPassword_ShouldReturnStatusCode(ForgotPasswordUser user, HttpStatusCode code)
    {
        await PerformCallAsync("/api/auth/forgotPassword", user, code);
    }

    [Theory]
    [ClassData(typeof(ResetPasswordUserGenerator))]
    public async Task ResetPassword_ShouldReturnStatusCode(ResetPasswordUser user, HttpStatusCode code)
    {
        await PerformCallAsync("/api/auth/resetPassword", user, code);
    }

    [Theory]
    [ClassData(typeof(ChangePasswordUserGenerator))]
    public async Task ChangePassword_ShouldReturnStatusCode(ChangePasswordUser user, HttpStatusCode code)
    {
        await PerformCallAsync("/api/auth/changePassword", user, code);
    }

    [Theory]
    [ClassData(typeof(DeleteUserGenerator))]
    public async Task DeleteUser_ShouldReturnStatusCode(LoginUser user, string email, HttpStatusCode code)
    {
        var loginRes = await _client.PostAsJsonAsync("/api/auth/login", user);

        string token = await loginRes.Content.ReadAsStringAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var res = await _client.DeleteAsync($"/api/auth?userEmail={email}");
        _client.DefaultRequestHeaders.Clear();

        Assert.Equal(code, res.StatusCode);
    }

    [Theory]
    [ClassData(typeof(LogoutUserGenerator))]
    public async Task Logout_ShouldReturnStatusCode(LoginUser user, string token, HttpStatusCode code)
    {
        var loginRes = await _client.PostAsJsonAsync("/api/auth/login", user);

        string loginToken = await loginRes.Content.ReadAsStringAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginToken);

        var res = await _client.GetAsync($"/api/auth/logout?token={(!string.IsNullOrEmpty(token) ? token : loginToken)}");
        _client.DefaultRequestHeaders.Clear();

        Assert.Equal(code, res.StatusCode);
    }

    [Theory]
    [ClassData(typeof(LockUserGenerator))]
    public async Task LockAndUnlockUserGetInfo_ShouldReturnStatusCode(LoginUser user, string userEmail, HttpStatusCode lockCode, HttpStatusCode unlockCode, HttpStatusCode resCode)
    {
        var loginRes = await _client.PostAsJsonAsync("/api/auth/login", user);

        string loginToken = await loginRes.Content.ReadAsStringAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginToken);

        var lockUserRes = await _client.GetAsync($"/api/auth/lockUser?userEmail={userEmail}");
        var unlockUserRes = await _client.GetAsync($"/api/auth/unlockUser?userEmail={userEmail}");
        var getInfoRes = await _client.GetAsync($"/api/auth/getUsers");
        _client.DefaultRequestHeaders.Clear();

        Assert.Equal(lockCode, lockUserRes.StatusCode);
        Assert.Equal(unlockCode, unlockUserRes.StatusCode);
        Assert.Equal(resCode, getInfoRes.StatusCode);
    }

    [Theory]
    [ClassData(typeof(ValidateTokenGenerator))]
    public async Task ValidateToken_ShouldReturnStatusCode(LoginUser user, string token, HttpStatusCode code)
    {
        var loginRes = await _client.PostAsJsonAsync("/api/auth/login", user);

        string loginToken = await loginRes.Content.ReadAsStringAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginToken);

        if (string.IsNullOrEmpty(token))
        {
            token = loginToken;
        }

        var getInfoRes = await _client.PostAsJsonAsync<dynamic>($"/api/auth/validateToken", new { Token = token });
        _client.DefaultRequestHeaders.Clear();

        Assert.Equal(code, getInfoRes.StatusCode);
    }

    private async Task PerformCallAsync<T>(string path, T user, HttpStatusCode code)
    {
        var res = await _client.PostAsJsonAsync(path, user);

        Assert.Equal(code, res.StatusCode);
    }

    private async Task CreateUserInAppAsync(RegisterUser user, bool confirmed)
    {
        var users = _ctx.Users.ToList();

        ApplicationUser appUser = new()
        {
            UserName = user.Email,
            Email = user.Email,
            EmailConfirmed = confirmed
        };

        if (!users.Any(u => u.UserName == user.Email))
        {
            IdentityResult creationRes = await _userManager.CreateAsync(appUser, user.Password);

            if (creationRes.Succeeded)
            {
                await _userManager.AddToRoleAsync(appUser, ApplicationConstants.Roles.User);

                if (user.Email == TestConstants.validUser.Email)
                {
                    await _userManager.AddToRoleAsync(appUser, ApplicationConstants.Roles.Admin);
                }

                if (confirmed && users.FirstOrDefault(u => u.UserName == user.Email) is ApplicationUser dbUser)
                {
                    await Task.Delay(200);
                    dbUser.EmailConfirmed = true;
                    await _ctx.SaveChangesAsync();
                }
            }
        }
    }
}
