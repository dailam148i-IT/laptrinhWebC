using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLSYLL.Infrastructure.Auth;
using QLSYLL.Infrastructure.Extensions;
using QLSYLL.Models;

namespace QLSYLL.Controllers;

[SessionAuthorize(RoleNames.SuperAdmin, RoleNames.HrAdmin, RoleNames.Manager, RoleNames.Employee)]
public class AnnouncementController(ApplicationDbContext dbContext) : Controller
{
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Quản lý thông báo";
        ViewData["ActiveMenu"] = "announcement";
        ViewData["Breadcrumb"] = "Thông báo";

        var query = dbContext.Announcements.Include(x => x.AuthorUser).AsQueryable();
        if (!HttpContext.IsInRole(RoleNames.SuperAdmin, RoleNames.HrAdmin))
        {
            query = query.Where(x => x.Status == "Đã đăng");
        }

        ViewBag.Announcements = await query
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new
            {
                x.Id,
                x.Title,
                Author = x.AuthorUser.FullName,
                CreatedAt = x.CreatedAt.ToString("dd/MM/yyyy"),
                x.Priority,
                x.Status
            })
            .ToListAsync();
        return View();
    }

    [SessionAuthorize(RoleNames.SuperAdmin, RoleNames.HrAdmin)]
    public IActionResult Create()
    {
        ViewData["Title"] = "Tạo thông báo mới";
        ViewData["ActiveMenu"] = "announcement";
        ViewData["Breadcrumb"] = "Tạo mới";
        ViewData["ParentPage"] = "Thông báo";
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [SessionAuthorize(RoleNames.SuperAdmin, RoleNames.HrAdmin)]
    public async Task<IActionResult> Create(IFormCollection form)
    {
        dbContext.Announcements.Add(new Announcement
        {
            Title = form["Title"]!,
            Content = form["Content"]!,
            Priority = form["Priority"]!,
            Status = form["Status"]!,
            AuthorUserId = HttpContext.GetCurrentUserId() ?? 1,
            CreatedAt = DateTime.UtcNow
        });

        await dbContext.SaveChangesAsync();
        TempData["SuccessMessage"] = "Tạo thông báo thành công!";
        return RedirectToAction(nameof(Index));
    }

    [SessionAuthorize(RoleNames.SuperAdmin, RoleNames.HrAdmin)]
    public async Task<IActionResult> Edit(int id)
    {
        var ann = await dbContext.Announcements.FirstOrDefaultAsync(x => x.Id == id);
        if (ann == null) return NotFound();

        ViewData["Title"] = "Chỉnh sửa thông báo";
        ViewData["ActiveMenu"] = "announcement";
        ViewData["Breadcrumb"] = "Chỉnh sửa";
        ViewData["ParentPage"] = "Thông báo";
        ViewBag.Announcement = ann;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [SessionAuthorize(RoleNames.SuperAdmin, RoleNames.HrAdmin)]
    public async Task<IActionResult> Edit(int id, IFormCollection form)
    {
        var ann = await dbContext.Announcements.FirstOrDefaultAsync(x => x.Id == id);
        if (ann == null) return NotFound();

        ann.Title = form["Title"]!;
        ann.Content = form["Content"]!;
        ann.Priority = form["Priority"]!;
        ann.Status = form["Status"]!;
        ann.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync();
        TempData["SuccessMessage"] = "Cập nhật thông báo thành công!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [SessionAuthorize(RoleNames.SuperAdmin, RoleNames.HrAdmin)]
    public async Task<IActionResult> Delete(int id)
    {
        var ann = await dbContext.Announcements.FirstOrDefaultAsync(x => x.Id == id);
        if (ann == null) return NotFound();

        dbContext.Announcements.Remove(ann);
        await dbContext.SaveChangesAsync();
        TempData["SuccessMessage"] = "Xóa thông báo thành công!";
        return RedirectToAction(nameof(Index));
    }
}
