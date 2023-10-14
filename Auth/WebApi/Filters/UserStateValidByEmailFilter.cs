using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OneOf.Types;
using WebApi.Services;

namespace WebApi.Filters
{
    public class UserStateValidByEmailFilter : IAsyncActionFilter
    {
        private readonly IAppUserFilterService _userFilterService;

        public UserStateValidByEmailFilter(IAppUserFilterService userFilterService)
        {
            _userFilterService = userFilterService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string? userEmail = context.ActionArguments.FirstOrDefault(a => a.Key == "userEmail").Value as string;

            var filteringRes = await _userFilterService.CheckUserStateAsync(userEmail);

            bool valid = filteringRes.Match(
                (BadRequestObjectResult res) =>
                {
                    context.Result = res;

                    return false;
                },
                (True t) =>
                {
                    return true;
                }
            );

            if (valid)
            {
                await next();
            }
            else
            {
                return;
            }
        }
    }
}
