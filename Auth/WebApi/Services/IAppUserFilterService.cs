using Microsoft.AspNetCore.Mvc;
using OneOf.Types;

namespace WebApi.Services;

public interface IAppUserFilterService
{
    Task<OneOf<BadRequestObjectResult, True>> CheckUserStateAsync(string? email);
}