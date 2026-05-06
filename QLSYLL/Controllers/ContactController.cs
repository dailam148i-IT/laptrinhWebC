using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLSYLL.Infrastructure.Auth;
using QLSYLL.Infrastructure.Extensions;
using QLSYLL.Models;

namespace QLSYLL.Controllers;

[SessionAuthorize(RoleNames.SuperAdmin, RoleNames.HrAdmin, RoleNames.Manager, RoleNames.Employee)]
public class ContactController(ApplicationDbContext dbContext) : Controller
{
    public async Task<IActionResult> Index(string? search, int? departmentId)
    {
        ViewData["Title"] = "Danh bạ nội bộ";
        ViewData["ActiveMenu"] = "contact";
        ViewData["Breadcrumb"] = "Danh bạ nội bộ";

        var showSensitive = HttpContext.IsInRole(RoleNames.SuperAdmin, RoleNames.HrAdmin);
        var query = dbContext.Employees
            .Where(x => !x.IsDeleted)
            .Include(x => x.Department)
            .Include(x => x.Position)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x => x.FullName.Contains(search) || x.Department.Name.Contains(search));
        }

        if (departmentId.HasValue)
        {
            query = query.Where(x => x.DepartmentId == departmentId.Value);
        }

        ViewBag.ShowSensitive = showSensitive;
        ViewBag.Departments = await dbContext.Departments.Where(x => !x.IsDeleted).OrderBy(x => x.Name).ToListAsync();
        ViewBag.Contacts = await query
            .OrderBy(x => x.FullName)
            .Select(x => new
            {
                x.Id,
                x.FullName,
                Email = x.PersonalEmail,
                x.Phone,
                Department = x.Department.Name,
                Position = x.Position.Name,
                BirthDate = x.BirthDate.ToString("dd/MM/yyyy"),
                x.IdentityNumber,
                Address = x.CurrentAddress,
                Salary = x.Salary.ToString("N0") + " ₫",
                x.FamilyInfo
            })
            .ToListAsync();

        return View();
    }
}
