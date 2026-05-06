using Microsoft.AspNetCore.Http;

namespace QLSYLL.Infrastructure.RequestContext;

public class HttpCurrentRequestContext(IHttpContextAccessor httpContextAccessor) : ICurrentRequestContext
{
    public int? UserId
    {
        get
        {
            var raw = httpContextAccessor.HttpContext?.Session.GetString("UserId");
            return int.TryParse(raw, out var userId) ? userId : null;
        }
    }

    public string? IpAddress => httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

    public string? Role => httpContextAccessor.HttpContext?.Session.GetString("Role");
}
