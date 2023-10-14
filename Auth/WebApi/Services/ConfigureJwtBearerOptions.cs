using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApi.Data;
using WebApi.Extensions;

namespace WebApi.Services
{
    //https://stackoverflow.com/questions/61186836/jwt-bearer-and-dependency-injection
    public class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly JwtOptions _jwtOptions;

        public ConfigureJwtBearerOptions(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public void Configure(string name, JwtBearerOptions options)
        {
            Configure(options);
        }

        public void Configure(JwtBearerOptions options)
        {
            SetJwtBearerOptions(options);
        }

        private void SetJwtBearerOptions(JwtBearerOptions opts)
        {
            opts.TokenValidationParameters = new()
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidAudience = _jwtOptions.Audience,
                ValidIssuer = _jwtOptions.Issuer,
                ValidAlgorithms = new List<string>() { SecurityAlgorithms.HmacSha512 },
                ClockSkew = TimeSpan.FromSeconds(_jwtOptions.ClockSkew ?? 30),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key)),
                NameClaimType = ApplicationConstants.UserNameClaimName,
                RoleClaimType = ApplicationConstants.RoleClaimName
            };

            opts.Events = new()
            {
                OnMessageReceived = (MessageReceivedContext ctx) =>
                {
                    ApplicationDbContext appCtx = ctx.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();

                    try
                    {
                        string? token = ctx.Request.Headers?.Authorization.FirstOrDefault(f => f.StartsWith("Bearer"));

                        if (token is not null)
                        {
                            string authToken = token.Split(" ")[1];
                            string hash = authToken.CreateMD5Hash();

                            if (appCtx.InvalidTokens.Any(t => t.TokenHash == hash))
                            {
                                ctx.Fail(new SecurityTokenValidationException());
                            }
                        }
                        else
                        {
                            ctx.Fail(new SecurityTokenValidationException());
                        }
                    }
                    catch (Exception)
                    {
                        ctx.Fail(new SecurityTokenValidationException());
                    }

                    return Task.CompletedTask;
                }
            };

            opts.MapInboundClaims = false;
        }

    }
}
