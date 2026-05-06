using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLSYLL.Infrastructure.Auth;
using QLSYLL.Infrastructure.Extensions;
using QLSYLL.Models;

namespace QLSYLL.Controllers;

[SessionAuthorize]
public class AdminController(ApplicationDbContext dbContext) : Controller
{
    [SessionAuthorize(RoleNames.SuperAdmin, RoleNames.HrAdmin, RoleNames.Manager, RoleNames.Employee)]
    public async Task<IActionResult> Dashboard()
    {
        var role = HttpContext.GetCurrentRole();
        if (role == RoleNames.Manager)
        {
            return RedirectToAction(nameof(DashboardDept));
        }

        if (role == RoleNames.Employee)
        {
            return RedirectToAction(nameof(DashboardSelf));
        }

        ViewData["Title"] = "Dashboard";
        ViewData["ActiveMenu"] = "dashboard";
        ViewData["Breadcrumb"] = "Dashboard";

        ViewBag.TotalEmployees = await dbContext.Employees.CountAsync(x => !x.IsDeleted);
        ViewBag.TotalDepartments = await dbContext.Departments.CountAsync(x => !x.IsDeleted);
        ViewBag.NewAnnouncements = await dbContext.Announcements.CountAsync(x =>
            x.Status == "Đã đăng" && x.CreatedAt >= DateTime.UtcNow.AddDays(-30));
        ViewBag.PendingResumes = await dbContext.Employees.CountAsync(x => !x.IsDeleted && x.Status == "Chờ duyệt");

        ViewBag.RecentActivities = await dbContext.AuditLogs
            .OrderByDescending(x => x.Timestamp)
            .Take(5)
            .Select(x => new
            {
                Color = x.Action == "DELETE" ? "amber" : x.Action == "LOGIN" ? "green" : "blue",
                Text = $"{x.Action} {x.TableName} #{x.RecordId}",
                Time = x.Timestamp.ToLocalTime().ToString("dd/MM/yyyy HH:mm")
            })
            .ToListAsync();

        ViewBag.RecentEmployees = await dbContext.Employees
            .Where(x => !x.IsDeleted)
            .Include(x => x.Department)
            .Include(x => x.Position)
            .OrderByDescending(x => x.CreatedAt)
            .Take(5)
            .Select(x => new
            {
                Name = x.FullName,
                Department = x.Department.Name,
                Position = x.Position.Name,
                Status = x.Status
            })
            .ToListAsync();

        return View();
    }

    [SessionAuthorize(RoleNames.SuperAdmin, RoleNames.HrAdmin, RoleNames.Manager)]
    public async Task<IActionResult> DashboardDept()
    {
        var userId = HttpContext.GetCurrentUserId();
        var role = HttpContext.GetCurrentRole();
        var currentResume = await dbContext.Employees
            .Include(x => x.Department)
            .FirstOrDefaultAsync(x => x.UserId == userId && !x.IsDeleted);
        var departmentId = currentResume?.DepartmentId;

        if (role == RoleNames.Manager && departmentId is null)
        {
            return RedirectToAction(nameof(DashboardSelf));
        }

        var resumesQuery = dbContext.Employees
            .Where(x => !x.IsDeleted && (role != RoleNames.Manager || x.DepartmentId == departmentId))
            .Include(x => x.Position)
            .Include(x => x.Department);

        ViewData["Title"] = "Dashboard phòng ban";
        ViewData["ActiveMenu"] = "dashboard";
        ViewData["Breadcrumb"] = "Dashboard phòng ban";

        ViewBag.EmployeeCount = await resumesQuery.CountAsync();
        ViewBag.PendingCount = await resumesQuery.CountAsync(x => x.Status == "Chờ duyệt");
        ViewBag.NewAnnouncements = await dbContext.Announcements.CountAsync(x =>
            x.Status == "Đã đăng" && x.CreatedAt >= DateTime.UtcNow.AddDays(-30));
        ViewBag.DepartmentEmployees = await resumesQuery
            .OrderBy(x => x.FullName)
            .Select(x => new
            {
                x.Id,
                x.FullName,
                Position = x.Position.Name,
                x.Status,
                Department = x.Department.Name
            })
            .ToListAsync();

        ViewBag.DepartmentName = currentResume?.Department?.Name ?? "Phòng ban";
        return View();
    }

    [SessionAuthorize(RoleNames.SuperAdmin, RoleNames.HrAdmin, RoleNames.Manager, RoleNames.Employee)]
    public async Task<IActionResult> DashboardSelf()
    {
        var userId = HttpContext.GetCurrentUserId();
        if (!userId.HasValue)
        {
            return RedirectToAction("Login", "Account");
        }

        var resume = await dbContext.Employees
            .Include(x => x.Department)
            .Include(x => x.Position)
            .FirstOrDefaultAsync(x => x.UserId == userId.Value && !x.IsDeleted);

        if (resume is null)
        {
            TempData["ErrorMessage"] = "Không tìm thấy hồ sơ cá nhân.";
            return RedirectToAction(nameof(Dashboard));
        }

        ViewData["Title"] = "Trang cá nhân";
        ViewData["ActiveMenu"] = "profile";
        ViewData["Breadcrumb"] = "Dashboard cá nhân";
        ViewBag.Resume = resume;
        ViewBag.RecentAnnouncements = await dbContext.Announcements
            .Where(x => x.Status == "Đã đăng")
            .OrderByDescending(x => x.CreatedAt)
            .Take(5)
            .ToListAsync();

        return View();
    }
}
