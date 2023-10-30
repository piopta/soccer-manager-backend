using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _ctx;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;
        private readonly FrontendOptions _frontendOptions;
        private readonly JwtOptions _jwtOptions;

        public AuthController(UserManager<ApplicationUser> userManager, ApplicationDbContext ctx, IMailService mailService,
                              IOptions<FrontendOptions> frontendOptions, IOptions<JwtOptions> jwtOptions, IMapper mapper)
        {
            _userManager = userManager;
            _ctx = ctx;
            _mailService = mailService;
            _mapper = mapper;
            _frontendOptions = frontendOptions.Value;
            _jwtOptions = jwtOptions.Value;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUser user)
        {
            if (!string.Equals(user.Password, user.ConfirmPassword))
            {
                return BadRequest("Password values must match");
            }

            ApplicationUser appUser = new()
            {
                UserName = user.Email,
                Email = user.Email
            };

            IdentityResult creationRes = await _userManager.CreateAsync(appUser, user.Password);

            if (!creationRes.Succeeded)
            {
                return BadRequest(string.Join(",", creationRes.Errors));
            }

            await _userManager.AddToRoleAsync(appUser, ApplicationConstants.Roles.User);

            string token = WebUtility.UrlEncode(await _userManager.GenerateEmailConfirmationTokenAsync(appUser));

            string link = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/Auth/confirmEmail?token={token}&email={appUser.Email}";

            MailInfo mail = new()
            {
                Parameters = new()
                {
                    {"@@link@@", link}
                },
                Subject = "[Soccer manager] - please verify your email address",
                TemplatePath = "EmailTemplates/email_confirmation.html",
                To = user.Email
            };

            _mailService.SendMail(mail);

            return Ok(new RegisterUserResult()
            {
                UserId = appUser.Id
            });
        }

        [HttpPost("login")]
        [ServiceFilter(typeof(UserStateValidFilter<LoginUser>))]
        public async Task<IActionResult> Login([FromBody] LoginUser user)
        {
            ApplicationUser appUser = await _userManager.FindByEmailAsync(user.Email);

            bool res = await _userManager.CheckPasswordAsync(appUser, user.Password);

            if (!res)
            {
                return BadRequest("User doesn't exist or password doesn't match");
            }

            JwtSecurityTokenHandler handler = new();

            List<Claim> claims = new() { new Claim(ApplicationConstants.UserNameClaimName, appUser.Email) };
            var userRoles = await _userManager.GetRolesAsync(appUser);

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ApplicationConstants.RoleClaimName, role));
            }

            ClaimsIdentity identity = new ClaimsIdentity(claims);

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));

            SecurityToken securityToken = handler.CreateToken(new SecurityTokenDescriptor()
            {
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                SigningCredentials = new SigningCredentials(algorithm: SecurityAlgorithms.HmacSha512, key: key),
                Subject = identity
            });

            string token = handler.WriteToken(securityToken);

            return Ok(new LoginUserResult()
            {
                Token = token
            });
        }

        [HttpPost("forgotPassword")]
        [ServiceFilter(typeof(UserStateValidFilter<ForgotPasswordUser>))]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordUser user)
        {
            ApplicationUser appUser = await _userManager.FindByEmailAsync(user.Email);

            string token = await _userManager.GeneratePasswordResetTokenAsync(appUser);
            string link = $"{_frontendOptions.AppUrl}/resetPassword?token={token}&email={appUser.Email}";

            MailInfo mail = new()
            {
                Parameters = new()
                {
                    {"@@link@@", link }
                },
                Subject = "[Soccer manager] - reset password request",
                TemplatePath = "EmailTemplates/forgot_password.html",
                To = user.Email
            };

            _mailService.SendMail(mail);

            return Ok(new ResponseResultGeneral("Password reset request sent"));
        }

        [HttpPost("resetPassword")]
        [ServiceFilter(typeof(UserStateValidFilter<ResetPasswordUser>))]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordUser user)
        {
            ApplicationUser appUser = await _userManager.FindByEmailAsync(user.Email);

            IdentityResult res = await _userManager.ResetPasswordAsync(appUser, user.Token, user.Password);

            if (!res.Succeeded)
            {
                return BadRequest(string.Join(",", res.Errors));
            }

            return Ok(new ResponseResultGeneral("Password successfully changed"));
        }

        [HttpPost("changePassword")]
        [ServiceFilter(typeof(UserStateValidFilter<ChangePasswordUser>))]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordUser user)
        {
            if (user.NewPassword != user.ConfirmedNewPassword)
            {
                return BadRequest("New password values must match");
            }

            ApplicationUser appUser = await _userManager.FindByEmailAsync(user.Email);

            IdentityResult res = await _userManager.ChangePasswordAsync(appUser, user.OldPassword, user.NewPassword);

            if (!res.Succeeded)
            {
                return BadRequest(string.Join(",", res.Errors));
            }

            return Ok(new ResponseResultGeneral("Password successfully changed"));
        }

        [HttpDelete]
        [Authorize]
        [ServiceFilter(typeof(ValidUserActionFilter))]
        public async Task<IActionResult> DeleteUser([FromQuery] string userEmail)
        {
            ApplicationUser appUser = await _userManager.FindByEmailAsync(userEmail);

            if (appUser is null)
            {
                return BadRequest("An error has occured while processing your request");
            }

            await _userManager.DeleteAsync(appUser);

            return NoContent();
        }

        [HttpGet("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromQuery] string token)
        {
            //here user is surely logged in - Authorize filter guarantees that
            ClaimsIdentity identity = HttpContext.User.Identities.FirstOrDefault()!;
            string userName = identity.Name!;

            ApplicationUser appUser = await _userManager.FindByNameAsync(userName);

            string hash = token.CreateMD5Hash();

            InvalidToken invalidToken = new()
            {
                TokenHash = hash,
                User = appUser,
                UserId = appUser.Id
            };

            await _ctx.InvalidTokens.AddAsync(invalidToken);

            await _ctx.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("lockUser")]
        [Authorize(Policy = ApplicationConstants.AdminAuthorizationPolicy)]
        public async Task<IActionResult> LockUser([FromQuery] string userEmail)
        {
            ApplicationUser appUser = await _userManager.FindByEmailAsync(userEmail);

            if (appUser is null)
            {
                return BadRequest("Required user doesn't exist");
            }

            await _userManager.SetLockoutEndDateAsync(appUser, new DateTimeOffset().AddDays(7));

            return NoContent();
        }

        [HttpGet("unlockUser")]
        [Authorize(Policy = ApplicationConstants.AdminAuthorizationPolicy)]
        public async Task<IActionResult> UnlockUser([FromQuery] string userEmail)
        {
            ApplicationUser appUser = await _userManager.FindByEmailAsync(userEmail);

            if (appUser is null)
            {
                return BadRequest("Required user doesn't exist");
            }

            await _userManager.SetLockoutEndDateAsync(appUser, null);

            return NoContent();
        }

        [HttpGet("getUsers")]
        [Authorize(Policy = ApplicationConstants.AdminAuthorizationPolicy)]
        public IActionResult GetUsersForAdministration()
        {
            List<ApplicationUser> users = _ctx.Users.ToList();
            List<ApplicationUserDTO> res = _mapper.Map<List<ApplicationUserDTO>>(users);

            return Ok(res);
        }

        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string email, [FromQuery] string token)
        {
            ApplicationUser appUser = await _userManager.FindByEmailAsync(email);

            IdentityResult res = await _userManager.ConfirmEmailAsync(appUser, token);

            if (!res.Succeeded)
            {
                return BadRequest("Email confirmation unsuccessful");
            }

            return Redirect($"{_frontendOptions.AppUrl}/login");
        }

        [HttpPost("validateToken")]
        public IActionResult ValidateToken([FromBody] ValidateToken validateToken)
        {
            string tokenHash = validateToken.Token.CreateMD5Hash();

            if (_ctx.InvalidTokens.Any(t => t.TokenHash == tokenHash))
            {
                return Unauthorized(new ValidateTokenResult()
                {
                    State = ValidateTokenState.INVALID
                });
            }

            TokenValidationResult validationRes = new JsonWebTokenHandler().ValidateToken(validateToken.Token, new()
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
            });

            ValidateTokenResult res = new()
            {
                State = validationRes.IsValid switch
                {
                    false => ValidateTokenState.INVALID,
                    _ => ValidateTokenState.VALID
                }
            };

            return res.State switch
            {
                ValidateTokenState.VALID => Ok(res),
                _ => Unauthorized(res)
            };
        }
    }
}
