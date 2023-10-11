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
                ClockSkew = TimeSpan.FromSeconds(_jwtOptions.ClockSkew ?? 30),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key))
            };

            opts.Events = new()
            {
                OnMessageReceived = (MessageReceivedContext ctx) =>
                {
                    ApplicationDbContext appCtx = ctx.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();

                    string token = ctx.Request.Headers.Authorization.FirstOrDefault(f => f.StartsWith("Bearer"));

                    string hash = token.CreateMD5Hash();

                    if (appCtx.InvalidTokens.Any(t => string.Equals(t.TokenHash, hash, StringComparison.OrdinalIgnoreCase)))
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
