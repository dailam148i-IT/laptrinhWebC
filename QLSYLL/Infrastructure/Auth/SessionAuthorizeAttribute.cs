using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using QLSYLL.Models;

namespace QLSYLL.Infrastructure.Auth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public sealed class SessionAuthorizeAttribute(params string[] roles) : Attribute, IAuthorizationFilter
{
    private readonly HashSet<string> _roles = roles.Length == 0 ? [] : [.. roles];

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var session = context.HttpContext.Session;
        var userId = session.GetString("UserId");
        var role = session.GetString("Role");

        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(role))
        {
            context.Result = new RedirectToActionResult("Login", "Account", null);
            return;
        }

        if (_roles.Count > 0 && !_roles.Contains(role))
        {
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
        }
    }
}
