using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLSYLL.Infrastructure.Extensions;
using QLSYLL.Models;

namespace QLSYLL.ViewComponents
{
    public class HeaderViewComponent(ApplicationDbContext dbContext) : ViewComponent
    {
        public IViewComponentResult Invoke(string breadcrumb = "", string parentPage = "")
        {
            ViewBag.Breadcrumb = breadcrumb;
            ViewBag.ParentPage = parentPage;
            var userName = HttpContext.Session.GetString("FullName") ?? "User";
            var userRole = HttpContext.Session.GetString("Role") ?? string.Empty;

            var announcementsQuery = dbContext.Announcements.Include(x => x.AuthorUser).AsQueryable();
            if (!HttpContext.IsInRole(RoleNames.SuperAdmin, RoleNames.HrAdmin))
            {
                announcementsQuery = announcementsQuery.Where(x => x.Status == "Đã đăng");
            }

            var notifications = announcementsQuery
                .OrderByDescending(x => x.CreatedAt)
                .Take(5)
                .Select(x => new
                {
                    x.Id,
                    x.Title,
                    Author = x.AuthorUser.FullName,
                    CreatedAt = x.CreatedAt.ToString("dd/MM/yyyy"),
                    x.Priority,
                    x.Status
                })
                .ToList();

            var notificationCount = announcementsQuery.Count();

            int? userId = null;
            var rawUserId = HttpContext.Session.GetString("UserId");
            if (int.TryParse(rawUserId, out var parsedUserId))
            {
                userId = parsedUserId;
            }

            var employeeId = dbContext.Employees
                .Where(x => x.UserId == userId && !x.IsDeleted)
                .Select(x => (int?)x.Id)
                .FirstOrDefault();

            ViewBag.UserName = userName;
            ViewBag.UserRole = userRole;
            ViewBag.UserInitial = userName[..1].ToUpperInvariant();
            ViewBag.NotificationCount = Math.Min(notificationCount, 9);
            ViewBag.Notifications = notifications;
            ViewBag.UserEmployeeId = employeeId;

            return View();
        }
    }
}