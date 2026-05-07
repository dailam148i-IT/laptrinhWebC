using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLSYLL.Infrastructure.Auth;
using QLSYLL.Infrastructure.Extensions;
using QLSYLL.Infrastructure.Services;
using QLSYLL.Models;
using QLSYLL.Models.ViewModels;

namespace QLSYLL.Controllers;

[SessionAuthorize(RoleNames.SuperAdmin)]
public class UserController(
    ApplicationDbContext dbContext,
    IAccountProvisioningService accountProvisioningService,
    AuditLogger auditLogger) : Controller
{
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Quản lý tài khoản";
        ViewData["ActiveMenu"] = "user";
        ViewData["Breadcrumb"] = "Tài khoản";

        ViewBag.Users = await dbContext.Users
            .Where(x => !x.IsDeleted)
            .Include(x => x.Role)
            .Include(x => x.Employee)
            .OrderBy(x => x.CreatedAt)
            .Select(x => new
            {
                x.Id,
                x.Username,
                x.FullName,
                x.Email,
                Role = x.Role.Code,
                x.IsActive,
                CreatedAt = x.CreatedAt.ToString("dd/MM/yyyy"),
                IsAdminAccount = x.Role.Code == RoleNames.SuperAdmin || x.Role.Code == RoleNames.HrAdmin,
                AccountType = x.Role.Code == RoleNames.SuperAdmin || x.Role.Code == RoleNames.HrAdmin
                    ? "Quản trị nội bộ"
                    : "Gắn với hồ sơ nhân sự",
                HasEmployeeProfile = x.Employee != null
            })
            .ToListAsync();

        return View();
    }

    public IActionResult Create()
    {
        PopulateAdminRoles();
        ViewData["Title"] = "Tạo tài khoản quản trị";
        ViewData["ActiveMenu"] = "user";
        ViewData["Breadcrumb"] = "Tạo mới";
        ViewData["ParentPage"] = "Tài khoản";
        return View(new AdminAccountCreateViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AdminAccountCreateViewModel model)
    {
        PopulateAdminRoles();
        ViewData["Title"] = "Tạo tài khoản quản trị";
        ViewData["ActiveMenu"] = "user";
        ViewData["Breadcrumb"] = "Tạo mới";
        ViewData["ParentPage"] = "Tài khoản";

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await accountProvisioningService.CreateAdminAccountAsync(model);
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage!);
            return View(model);
        }

        TempData["SuccessMessage"] = "Tạo tài khoản quản trị thành công!";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var user = await dbContext.Users
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        if (user is null || !RoleNames.IsAdminRole(user.Role.Code))
        {
            return NotFound();
        }

        PopulateAdminRoles();
        ViewData["Title"] = "Chỉnh sửa tài khoản quản trị";
        ViewData["ActiveMenu"] = "user";
        ViewData["Breadcrumb"] = "Chỉnh sửa";
        ViewData["ParentPage"] = "Tài khoản";

        return View(new AdminAccountEditViewModel
        {
            Id = user.Id,
            Username = user.Username,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.Code
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, AdminAccountEditViewModel model)
    {
        PopulateAdminRoles();
        ViewData["Title"] = "Chỉnh sửa tài khoản quản trị";
        ViewData["ActiveMenu"] = "user";
        ViewData["Breadcrumb"] = "Chỉnh sửa";
        ViewData["ParentPage"] = "Tài khoản";

        if (id != model.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await accountProvisioningService.UpdateAdminAccountAsync(id, model);
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage!);
            return View(model);
        }

        TempData["SuccessMessage"] = "Cập nhật tài khoản quản trị thành công!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [SessionAuthorize(RoleNames.SuperAdmin)]
    public async Task<IActionResult> Delete(int id)
    {
        var currentUserId = HttpContext.GetCurrentUserId();
        var user = await dbContext.Users
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        if (user is null)
        {
            return NotFound();
        }

        if (!RoleNames.IsAdminRole(user.Role.Code))
        {
            TempData["ErrorMessage"] = "Tài khoản gắn với hồ sơ nhân sự chỉ được vô hiệu hóa, không xóa tại màn hình này.";
            return RedirectToAction(nameof(Index));
        }

        if (currentUserId == id)
        {
            TempData["ErrorMessage"] = "Không thể xóa tài khoản đang đăng nhập.";
            return RedirectToAction(nameof(Index));
        }

        if (user.Role.Code == RoleNames.SuperAdmin &&
            await dbContext.Users.CountAsync(x => x.RoleId == user.RoleId && !x.IsDeleted) == 1)
        {
            TempData["ErrorMessage"] = "Không thể xóa SuperAdmin cuối cùng.";
            return RedirectToAction(nameof(Index));
        }

        user.IsDeleted = true;
        user.IsActive = false;
        user.UpdatedAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync();
        await auditLogger.LogAsync(
            "Users",
            "DELETE",
            user.Id.ToString(),
            newValues: "{\"SoftDeleted\":true}",
            userId: currentUserId);

        TempData["SuccessMessage"] = "Xóa tài khoản quản trị thành công!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleActive(int id)
    {
        var result = await accountProvisioningService.ToggleActiveAsync(id);
        if (!result.Succeeded)
        {
            TempData["ErrorMessage"] = result.ErrorMessage;
            return RedirectToAction(nameof(Index));
        }

        TempData["SuccessMessage"] = "Cập nhật trạng thái tài khoản thành công!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(int id)
    {
        var result = await accountProvisioningService.ResetPasswordAsync(id);
        if (!result.Succeeded)
        {
            TempData["ErrorMessage"] = result.ErrorMessage;
            return RedirectToAction(nameof(Index));
        }

        var user = await dbContext.Users
            .Where(x => x.Id == id)
            .Select(x => x.Username)
            .FirstOrDefaultAsync();

        TempData["SuccessMessage"] = $"Mật khẩu mới của {user}: {result.GeneratedPassword}";
        return RedirectToAction(nameof(Index));
    }

    private void PopulateAdminRoles()
    {
        ViewBag.Roles = RoleNames.AdminRoles
            .Select(role => new SelectListItem(GetRoleLabel(role), role))
            .ToList();
    }

    private static string GetRoleLabel(string roleCode)
    {
        return roleCode switch
        {
            RoleNames.SuperAdmin => "SA - Super Admin",
            RoleNames.HrAdmin => "HR - Nhân sự",
            _ => roleCode
        };
    }
}
