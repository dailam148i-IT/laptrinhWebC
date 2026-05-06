using Microsoft.AspNetCore.Mvc;
using QLSYLL.Models;

namespace QLSYLL.ViewComponents
{
    /// <summary>
    /// ViewComponent: Top header bar với breadcrumb, notification bell, user info
    /// Sử dụng: @await Component.InvokeAsync("Header", new { breadcrumb = "Dashboard" })
    /// </summary>
    public class HeaderViewComponent(ApplicationDbContext dbContext) : ViewComponent
    {
        public IViewComponentResult Invoke(string breadcrumb = "", string parentPage = "")
        {
            ViewBag.Breadcrumb = breadcrumb;
            ViewBag.ParentPage = parentPage;
            var userName = HttpContext.Session.GetString("FullName") ?? "User";
            var userRole = HttpContext.Session.GetString("Role") ?? string.Empty;
            var notificationCount = dbContext.Announcements.Count(x => x.Status == "Đã đăng");

            ViewBag.UserName = userName;
            ViewBag.UserRole = userRole;
            ViewBag.UserInitial = userName[..1].ToUpperInvariant();
            ViewBag.NotificationCount = Math.Min(notificationCount, 9);
            return View();
        }
    }
}
