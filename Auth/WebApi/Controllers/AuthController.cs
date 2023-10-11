using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _ctx;
        private readonly IMailService _mailService;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
                              ApplicationDbContext ctx, IMailService mailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _ctx = ctx;
            _mailService = mailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUser user)
        {
            ApplicationUser appUser = new()
            {
                UserName = user.Email,
                Email = user.Email
            };

            IdentityResult creationRes = await _userManager.CreateAsync(appUser, user.Password);

            if (!creationRes.Succeeded)
            {
                return BadRequest(creationRes.Errors);
            }

            await _userManager.AddToRoleAsync(appUser, ApplicationConstants.Roles.User);

            string token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);

            MailInfo mail = new()
            {
                Parameters = new()
                {
                    {"@@token@@", token }
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
    }
}
