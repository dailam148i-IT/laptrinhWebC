using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLSYLL.Infrastructure.Auth;
using QLSYLL.Models;

namespace QLSYLL.Controllers;

[SessionAuthorize(RoleNames.SuperAdmin, RoleNames.HrAdmin)]
public class DepartmentController(ApplicationDbContext dbContext) : Controller
{
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Quản lý phòng ban";
        ViewData["ActiveMenu"] = "department";
        ViewData["Breadcrumb"] = "Phòng ban";
        ViewBag.Departments = await dbContext.Departments
            .Where(x => !x.IsDeleted)
            .Include(x => x.Manager)
            .Select(x => new
            {
                x.Id,
                x.Name,
                x.Code,
                ManagerName = x.Manager != null ? x.Manager.FullName : "Chưa gán",
                EmployeeCount = x.Employees.Count(r => !r.IsDeleted),
                CreatedAt = x.CreatedAt.ToString("dd/MM/yyyy")
            })
            .ToListAsync();
        return View();
    }

    [SessionAuthorize(RoleNames.SuperAdmin, RoleNames.HrAdmin)]
    public async Task<IActionResult> Create()
    {
        await PopulateManagersAsync();
        ViewData["Title"] = "Thêm phòng ban";
        ViewData["ActiveMenu"] = "department";
        ViewData["Breadcrumb"] = "Thêm mới";
        ViewData["ParentPage"] = "Phòng ban";
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [SessionAuthorize(RoleNames.SuperAdmin, RoleNames.HrAdmin)]
    public async Task<IActionResult> Create(IFormCollection form)
    {
        var code = form["Code"].ToString().Trim();
        if (await dbContext.Departments.AnyAsync(x => x.Code == code && !x.IsDeleted))
        {
            TempData["ErrorMessage"] = "Mã phòng ban đã tồn tại.";
            return RedirectToAction(nameof(Create));
        }

        dbContext.Departments.Add(new Department
        {
            Name = form["Name"]!,
            Code = code,
            Description = form["Description"],
            ManagerId = int.TryParse(form["ManagerId"], out var managerId) ? managerId : null,
            CreatedAt = DateTime.UtcNow
        });

        await dbContext.SaveChangesAsync();
        TempData["SuccessMessage"] = "Thêm phòng ban thành công!";
        return RedirectToAction(nameof(Index));
    }

    [SessionAuthorize(RoleNames.SuperAdmin, RoleNames.HrAdmin)]
    public async Task<IActionResult> Edit(int id)
    {
        var dept = await dbContext.Departments.Include(x => x.Manager).FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (dept == null) return NotFound();

        await PopulateManagersAsync();
        ViewData["Title"] = "Chỉnh sửa phòng ban";
        ViewData["ActiveMenu"] = "department";
        ViewData["Breadcrumb"] = "Chỉnh sửa";
        ViewData["ParentPage"] = "Phòng ban";
        ViewBag.Department = new
        {
            dept.Id,
            dept.Name,
            dept.Code,
            ManagerName = dept.Manager?.FullName ?? "Chưa gán"
        };
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [SessionAuthorize(RoleNames.SuperAdmin, RoleNames.HrAdmin)]
    public async Task<IActionResult> Edit(int id, IFormCollection form)
    {
        var dept = await dbContext.Departments.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (dept == null) return NotFound();

        dept.Name = form["Name"]!;
        dept.Code = form["Code"]!;
        dept.ManagerId = int.TryParse(form["ManagerId"], out var managerId) ? managerId : null;
        dept.Description = form["Description"];

        await dbContext.SaveChangesAsync();
        TempData["SuccessMessage"] = "Cập nhật phòng ban thành công!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [SessionAuthorize(RoleNames.SuperAdmin, RoleNames.HrAdmin)]
    public async Task<IActionResult> Delete(int id)
    {
        var dept = await dbContext.Departments.Include(x => x.Employees).FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (dept == null) return NotFound();

        if (dept.Employees.Any(x => !x.IsDeleted))
        {
            TempData["ErrorMessage"] = "Vui lòng chuyển hết nhân viên sang phòng khác trước khi xóa.";
            return RedirectToAction(nameof(Index));
        }

        dept.IsDeleted = true;
        await dbContext.SaveChangesAsync();
        TempData["SuccessMessage"] = "Xóa phòng ban thành công!";
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateManagersAsync()
    {
        ViewBag.Managers = await dbContext.Users
            .Include(x => x.Role)
            .Where(x => !x.IsDeleted && x.Role.Code == RoleNames.Manager)
            .OrderBy(x => x.FullName)
            .ToListAsync();
    }
}
