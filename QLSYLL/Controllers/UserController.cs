using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLSYLL.Infrastructure.Auth;
using QLSYLL.Infrastructure.Extensions;
using QLSYLL.Infrastructure.Security;
using QLSYLL.Models;

namespace QLSYLL.Controllers;

[SessionAuthorize(RoleNames.SuperAdmin)]
public class UserController(ApplicationDbContext dbContext) : Controller
{
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Quản lý tài khoản";
        ViewData["ActiveMenu"] = "user";
        ViewData["Breadcrumb"] = "Tài khoản";
        ViewBag.Users = await dbContext.Users
            .Where(x => !x.IsDeleted)
            .Include(x => x.Role)
            .OrderBy(x => x.CreatedAt)
            .Select(x => new
            {
                x.Id,
                x.Username,
                x.FullName,
                x.Email,
                Role = x.Role.Code,
                x.IsActive,
                CreatedAt = x.CreatedAt.ToString("dd/MM/yyyy")
            })
            .ToListAsync();
        return View();
    }

    public IActionResult Create()
    {
        ViewData["Title"] = "Tạo tài khoản mới";
        ViewData["ActiveMenu"] = "user";
        ViewData["Breadcrumb"] = "Tạo mới";
        ViewData["ParentPage"] = "Tài khoản";
        ViewBag.Roles = GetAssignableRoles();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(IFormCollection form)
    {
        var role = form["Role"].ToString();
        if (!GetAssignableRoles().Contains(role))
        {
            return Forbid();
        }

        var username = form["Username"]!.ToString().Trim();
        var email = form["Email"]!.ToString().Trim();
        if (await dbContext.Users.AnyAsync(x => x.Username == username || x.Email == email))
        {
            TempData["ErrorMessage"] = "Tên đăng nhập hoặc email đã tồn tại.";
            return RedirectToAction(nameof(Create));
        }

        dbContext.Users.Add(new User
        {
            Username = username,
            FullName = form["FullName"]!,
            Email = email,
            RoleId = GetRoleId(role),
            PasswordHash = PasswordHasher.HashPassword(form["Password"]!),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        });

        await dbContext.SaveChangesAsync();
        TempData["SuccessMessage"] = "Tạo tài khoản thành công!";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var user = await dbContext.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (user == null) return NotFound();

        ViewData["Title"] = "Chỉnh sửa tài khoản";
        ViewData["ActiveMenu"] = "user";
        ViewData["Breadcrumb"] = "Chỉnh sửa";
        ViewData["ParentPage"] = "Tài khoản";
        ViewBag.UserItem = user;
        ViewBag.Roles = GetAssignableRoles(user.Role.Code);
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, IFormCollection form)
    {
        var user = await dbContext.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (user == null) return NotFound();

        var role = form["Role"].ToString();
        if (!GetAssignableRoles(user.Role.Code).Contains(role))
        {
            return Forbid();
        }

        user.FullName = form["FullName"]!;
        user.Email = form["Email"]!;
        user.RoleId = GetRoleId(role);
        user.UpdatedAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync();
        TempData["SuccessMessage"] = "Cập nhật tài khoản thành công!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [SessionAuthorize(RoleNames.SuperAdmin)]
    public async Task<IActionResult> Delete(int id)
    {
        var currentUserId = HttpContext.GetCurrentUserId();
        var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (user == null) return NotFound();

        if (currentUserId == id)
        {
            TempData["ErrorMessage"] = "Không thể xóa tài khoản đang đăng nhập.";
            return RedirectToAction(nameof(Index));
        }

        if (user.RoleId == GetRoleId(RoleNames.SuperAdmin) && await dbContext.Users.CountAsync(x => x.RoleId == GetRoleId(RoleNames.SuperAdmin) && !x.IsDeleted) == 1)
        {
            TempData["ErrorMessage"] = "Không thể xóa SuperAdmin cuối cùng.";
            return RedirectToAction(nameof(Index));
        }

        user.IsDeleted = true;
        user.IsActive = false;
        await dbContext.SaveChangesAsync();
        TempData["SuccessMessage"] = "Xóa tài khoản thành công!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleActive(int id)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (user == null) return NotFound();

        user.IsActive = !user.IsActive;
        user.UpdatedAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync();
        TempData["SuccessMessage"] = "Cập nhật trạng thái tài khoản thành công!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(int id)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (user == null) return NotFound();

        var newPassword = $"Reset@{Random.Shared.Next(100000, 999999)}";
        user.PasswordHash = PasswordHasher.HashPassword(newPassword);
        user.UpdatedAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync();
        TempData["SuccessMessage"] = $"Mật khẩu mới của {user.Username}: {newPassword}";
        return RedirectToAction(nameof(Index));
    }

    private List<string> GetAssignableRoles(string? currentRole = null)
    {
        return [RoleNames.SuperAdmin, RoleNames.HrAdmin, RoleNames.Manager, RoleNames.Employee];
    }

    private static int GetRoleId(string roleCode)
    {
        return roleCode switch
        {
            RoleNames.SuperAdmin => 1,
            RoleNames.HrAdmin => 2,
            RoleNames.Manager => 3,
            _ => 4
        };
    }
}
