using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace WebApi.Filters
{
    public class ValidUserActionFilter : IAsyncActionFilter
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ValidUserActionFilter(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            ClaimsIdentity? identity = context.HttpContext.User.Identities.FirstOrDefault();

            if (identity is null)
            {
                context.Result = new BadRequestObjectResult("Cannot delete given user");
                return;
            }

            string userName = identity.Name!;

            string? deletedUserName = context.ActionArguments.FirstOrDefault(a => a.Key == "userEmail").Value as string;

            ApplicationUser? user = await _userManager.FindByEmailAsync(userName);

            if (user is null)
            {
                context.Result = new BadRequestObjectResult("Cannot delete given user");
                return;
            }

            IList<string> userRoles = await _userManager.GetRolesAsync(user);

            if (string.Equals(userName, deletedUserName) || userRoles.Contains(ApplicationConstants.Roles.Admin))
            {
                await next();
            }

            context.Result = new BadRequestObjectResult("Cannot delete given user");
            return;
        }
    }
}
