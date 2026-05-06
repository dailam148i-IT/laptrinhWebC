using QLSYLL.Infrastructure.RequestContext;
using QLSYLL.Models;

namespace QLSYLL.Infrastructure.Services;

public class AuditLogger(ApplicationDbContext dbContext, ICurrentRequestContext currentRequestContext)
{
    public async Task LogAsync(
        string tableName,
        string action,
        string? recordId = null,
        string? oldValues = null,
        string? newValues = null,
        int? userId = null)
    {
        dbContext.AuditLogs.Add(new AuditLog
        {
            TableName = tableName,
            Action = action,
            RecordId = recordId,
            OldValues = oldValues,
            NewValues = newValues,
            UserId = userId ?? currentRequestContext.UserId,
            IpAddress = currentRequestContext.IpAddress,
            Timestamp = DateTime.UtcNow
        });

        await dbContext.SaveChangesAsync();
    }
}
