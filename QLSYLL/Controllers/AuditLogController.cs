using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLSYLL.Infrastructure.Auth;
using QLSYLL.Models;

namespace QLSYLL.Controllers;

[SessionAuthorize(RoleNames.SuperAdmin)]
public class AuditLogController(ApplicationDbContext dbContext) : Controller
{
    public async Task<IActionResult> Index([FromQuery(Name = "action")] string? actionFilter)
    {
        ViewData["Title"] = "Audit Log";
        ViewData["ActiveMenu"] = "auditlog";
        ViewData["Breadcrumb"] = "Audit Log";

        var query = dbContext.AuditLogs.Include(x => x.User).AsQueryable();
        if (!string.IsNullOrWhiteSpace(actionFilter))
        {
            query = query.Where(x => x.Action == actionFilter);
        }

        ViewBag.Logs = await query
            .OrderByDescending(x => x.Timestamp)
            .Take(200)
            .Select(x => new
            {
                x.Id,
                UserName = x.User != null ? x.User.FullName : "Hệ thống",
                x.TableName,
                x.RecordId,
                x.Action,
                x.OldValues,
                x.NewValues,
                Timestamp = x.Timestamp.ToLocalTime().ToString("dd/MM/yyyy HH:mm"),
                x.IpAddress
            })
            .ToListAsync();

        return View();
    }
}
