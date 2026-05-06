using Microsoft.AspNetCore.Mvc;

namespace QLSYLL.ViewComponents
{
    /// <summary>
    /// ViewComponent: Sidebar navigation cho Admin Layout
    /// Sử dụng: @await Component.InvokeAsync("Sidebar", new { activeMenu = "dashboard" })
    /// </summary>
    public class SidebarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string activeMenu = "")
        {
            ViewBag.ActiveMenu = activeMenu;
            ViewBag.Role = HttpContext.Session.GetString("Role") ?? string.Empty;
            return View();
        }
    }
}
