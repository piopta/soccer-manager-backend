using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using WebApi.Filters;

namespace WebApi.ServicesRegistration;

public static class AuthServicesRegistration
{
    public static void AddAuthServices(this IServiceCollection services)
    {
        //https://referbruv.com/blog/upgrading-to-net-core-30-jwt-bearer-with-strongly-typed-configuration/ -> using JwtOptions with Jwt bearer registration

        services.AddIdentity<ApplicationUser, ApplicationRole>(opts =>
        {
            opts.Password.RequiredLength = ApplicationConstants.Validation.MinimumPasswordLength;
        })
                .AddRoles<ApplicationRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();


        services.AddAuthentication(opts =>
        {
            opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer();

        services.ConfigureOptions<ConfigureJwtBearerOptions>();
        services.AddAuthorization(SetAuthorizationOptions);

        //registration explained here: https://stackoverflow.com/questions/49352434/how-to-register-interface-with-generic-type-in-startup-cs#:~:text=Open%20generics%20are%20always%20specified%20the%20same%20way,parameters%2C%20you%20would%20specify%20how%20many%20using%20commas.
        services.AddScoped(typeof(UserStateValidFilter<>));
        services.AddScoped<UserStateValidByEmailFilter>();
        services.AddScoped<ValidUserActionFilter>();
        services.AddScoped<IAppUserFilterService, AppUserFilterService>();
    }

    private static void SetAuthorizationOptions(AuthorizationOptions opts)
    {
        opts.DefaultPolicy = new AuthorizationPolicyBuilder()
            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .RequireClaim(ApplicationConstants.RoleClaimName, ApplicationConstants.Roles.User)
            .Build();

        opts.AddPolicy(ApplicationConstants.AdminAuthorizationPolicy, policyBuilder =>
        {
            policyBuilder.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                                 .RequireAuthenticatedUser()
                                 .RequireClaim(ApplicationConstants.RoleClaimName, ApplicationConstants.Roles.User)
                                 .RequireClaim(ApplicationConstants.RoleClaimName, ApplicationConstants.Roles.Admin);
        });
    }
}
