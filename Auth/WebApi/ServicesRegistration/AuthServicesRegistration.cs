using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using WebApi.Data;
using WebApi.Services;

namespace WebApi.ServicesRegistration
{
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

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer();
            services.ConfigureOptions<ConfigureJwtBearerOptions>();
            services.AddAuthorization(SetAuthorizationOptions);
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
}
