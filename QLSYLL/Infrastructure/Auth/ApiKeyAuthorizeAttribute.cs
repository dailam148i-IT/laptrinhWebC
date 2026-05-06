using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace QLSYLL.Infrastructure.Auth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class ApiKeyAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var expectedApiKey = configuration["ApiSettings:ApiKey"];
        var actualApiKey = context.HttpContext.Request.Headers["X-API-Key"].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(expectedApiKey) || expectedApiKey != actualApiKey)
        {
            context.Result = new UnauthorizedObjectResult(new
            {
                success = false,
                message = "API key không hợp lệ."
            });
        }
    }
}
