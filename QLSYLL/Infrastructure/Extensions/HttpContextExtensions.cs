using Microsoft.AspNetCore.Http;
using QLSYLL.Models;

namespace QLSYLL.Infrastructure.Extensions;

public static class HttpContextExtensions
{
    public static int? GetCurrentUserId(this HttpContext httpContext)
    {
        var raw = httpContext.Session.GetString("UserId");
        return int.TryParse(raw, out var userId) ? userId : null;
    }

    public static string? GetCurrentRole(this HttpContext httpContext)
    {
        return httpContext.Session.GetString("Role");
    }

    public static bool IsInRole(this HttpContext httpContext, params string[] roles)
    {
        var role = httpContext.GetCurrentRole();
        return !string.IsNullOrWhiteSpace(role) && roles.Contains(role);
    }

    public static void SignIn(this HttpContext httpContext, User user)
    {
        httpContext.Session.SetString("UserId", user.Id.ToString());
        httpContext.Session.SetString("Username", user.Username);
        httpContext.Session.SetString("FullName", user.FullName);
        httpContext.Session.SetString("Role", user.Role.Code);
    }
}
