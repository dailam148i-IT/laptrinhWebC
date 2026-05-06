using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLSYLL.Infrastructure.Auth;
using QLSYLL.Models;

namespace QLSYLL.Controllers;

[SessionAuthorize(RoleNames.SuperAdmin, RoleNames.HrAdmin)]
public class PositionController(ApplicationDbContext dbContext) : Controller
{
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Quản lý chức vụ";
        ViewData["ActiveMenu"] = "position";
        ViewData["Breadcrumb"] = "Chức vụ";
        ViewBag.Positions = await dbContext.Positions
            .Where(x => !x.IsDeleted)
            .Select(x => new
            {
                x.Id,
                x.Name,
                x.Level,
                BaseSalary = x.BaseSalary.ToString("N0") + " ₫",
                EmployeeCount = x.Employees.Count(r => !r.IsDeleted),
                x.Description
            })
            .OrderBy(x => x.Level)
            .ToListAsync();
        return View();
    }

    [SessionAuthorize(RoleNames.SuperAdmin, RoleNames.HrAdmin)]
    public IActionResult Create()
    {
        ViewData["Title"] = "Thêm chức vụ";
        ViewData["ActiveMenu"] = "position";
        ViewData["Breadcrumb"] = "Thêm mới";
        ViewData["ParentPage"] = "Chức vụ";
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [SessionAuthorize(RoleNames.SuperAdmin, RoleNames.HrAdmin)]
    public async Task<IActionResult> Create(IFormCollection form)
    {
        dbContext.Positions.Add(new Position
        {
            Name = form["Name"]!,
            Level = int.TryParse(form["Level"], out var level) ? level : 1,
            BaseSalary = decimal.TryParse(form["BaseSalary"].ToString().Replace(",", "").Replace("₫", "").Trim(), out var salary) ? salary : 0,
            Description = form["Description"]
        });

        await dbContext.SaveChangesAsync();
        TempData["SuccessMessage"] = "Thêm chức vụ thành công!";
        return RedirectToAction(nameof(Index));
    }

    [SessionAuthorize(RoleNames.SuperAdmin, RoleNames.HrAdmin)]
    public async Task<IActionResult> Edit(int id)
    {
        var pos = await dbContext.Positions.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (pos == null) return NotFound();

        ViewData["Title"] = "Chỉnh sửa chức vụ";
        ViewData["ActiveMenu"] = "position";
        ViewData["Breadcrumb"] = "Chỉnh sửa";
        ViewData["ParentPage"] = "Chức vụ";
        ViewBag.Position = new
        {
            pos.Id,
            pos.Name,
            pos.Level,
            BaseSalary = pos.BaseSalary.ToString("N0"),
            pos.Description
        };
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [SessionAuthorize(RoleNames.SuperAdmin, RoleNames.HrAdmin)]
    public async Task<IActionResult> Edit(int id, IFormCollection form)
    {
        var pos = await dbContext.Positions.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (pos == null) return NotFound();

        pos.Name = form["Name"]!;
        pos.Level = int.TryParse(form["Level"], out var level) ? level : pos.Level;
        pos.BaseSalary = decimal.TryParse(form["BaseSalary"].ToString().Replace(",", "").Replace("₫", "").Trim(), out var salary) ? salary : pos.BaseSalary;
        pos.Description = form["Description"];

        await dbContext.SaveChangesAsync();
        TempData["SuccessMessage"] = "Cập nhật chức vụ thành công!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [SessionAuthorize(RoleNames.SuperAdmin, RoleNames.HrAdmin)]
    public async Task<IActionResult> Delete(int id)
    {
        var pos = await dbContext.Positions.Include(x => x.Employees).FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (pos == null) return NotFound();

        if (pos.Employees.Any(x => !x.IsDeleted))
        {
            TempData["ErrorMessage"] = "Không thể xóa chức vụ đang có nhân viên sử dụng.";
            return RedirectToAction(nameof(Index));
        }

        pos.IsDeleted = true;
        await dbContext.SaveChangesAsync();
        TempData["SuccessMessage"] = "Xóa chức vụ thành công!";
        return RedirectToAction(nameof(Index));
    }
}
