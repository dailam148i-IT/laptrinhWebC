namespace QLSYLL.Infrastructure.RequestContext;

public interface ICurrentRequestContext
{
    int? UserId { get; }
    string? IpAddress { get; }
    string? Role { get; }
}
