using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OneOf.Types;
using WebApi.Services;

namespace WebApi.Filters
{
    public class UserStateValidFilter<T> : IAsyncActionFilter where T : BaseUser
    {
        private readonly IAppUserFilterService _userFilterService;

        public UserStateValidFilter(IAppUserFilterService userFilterService)
        {
            _userFilterService = userFilterService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            T? user = context.ActionArguments.FirstOrDefault(a => a.Key == "user").Value as T;

            var filteringRes = await _userFilterService.CheckUserStateAsync(user?.Email);

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
