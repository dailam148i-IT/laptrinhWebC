using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLSYLL.Infrastructure.Extensions;
using QLSYLL.Infrastructure.Security;
using QLSYLL.Infrastructure.Services;
using QLSYLL.Models;
using QLSYLL.Models.ViewModels;

namespace QLSYLL.Controllers
{
    /// <summary>
    /// Controller xử lý Authentication: Đăng nhập, Đăng xuất, Quên mật khẩu
    /// </summary>
    public class AccountController(ApplicationDbContext dbContext, AuditLogger auditLogger) : Controller
    {
        // GET: /Account/Login
        public IActionResult Login()
        {
            // Nếu đã đăng nhập → chuyển về Dashboard
            if (HttpContext.Session.GetString("UserId") != null)
            {
                return RedirectToRoleDashboard(HttpContext.Session.GetString("Role"));
            }
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = dbContext.Users
                .Include(x => x.Role)
                .AsNoTracking()
                .FirstOrDefault(x => x.Username == model.Username && !x.IsDeleted);

            if (user is null || !PasswordHasher.VerifyPassword(model.Password, user.PasswordHash))
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                return View(model);
            }

            if (!user.IsActive)
            {
                ModelState.AddModelError("", "Tài khoản đã bị khóa.");
                return View(model);
            }

            HttpContext.SignIn(user);
            await auditLogger.LogAsync("Users", "LOGIN", user.Id.ToString(), newValues: $"{{\"Username\":\"{user.Username}\"}}", userId: user.Id);

            TempData["SuccessMessage"] = "Đăng nhập thành công!";
            return RedirectToRoleDashboard(user.Role.Code);
        }

        // GET: /Account/Logout
        public async Task<IActionResult> Logout()
        {
            var userId = HttpContext.GetCurrentUserId();
            if (userId.HasValue)
            {
                await auditLogger.LogAsync("Users", "LOGOUT", userId.Value.ToString(), userId: userId);
            }

            HttpContext.Session.Clear();
            TempData["InfoMessage"] = "Bạn đã đăng xuất.";
            return RedirectToAction("Login");
        }

        // GET: /Account/ForgotPassword
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST: /Account/ForgotPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = dbContext.Users.FirstOrDefault(x =>
                x.Username == model.Username &&
                x.Email == model.Email &&
                !x.IsDeleted);

            if (user is null)
            {
                ModelState.AddModelError("", "Không tìm thấy tài khoản khớp với thông tin đã nhập.");
                return View(model);
            }

            var newPassword = $"Tmp@{Random.Shared.Next(100000, 999999)}";
            user.PasswordHash = PasswordHasher.HashPassword(newPassword);
            user.UpdatedAt = DateTime.UtcNow;
            await dbContext.SaveChangesAsync();
            await auditLogger.LogAsync("Users", "UPDATE", user.Id.ToString(), newValues: $"{{\"ResetPassword\":\"{newPassword}\"}}", userId: user.Id);

            TempData["SuccessMessage"] = $"Mật khẩu tạm thời của bạn là: {newPassword}";
            return RedirectToAction("Login");
        }

        // GET: /Account/ChangePassword
        public IActionResult ChangePassword()
        {
            ViewData["Title"] = "Đổi mật khẩu";
            ViewData["ActiveMenu"] = "changepassword";
            ViewData["Breadcrumb"] = "Đổi mật khẩu";
            return View();
        }

        // POST: /Account/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            ViewData["Title"] = "Đổi mật khẩu";
            ViewData["ActiveMenu"] = "changepassword";
            ViewData["Breadcrumb"] = "Đổi mật khẩu";

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = HttpContext.GetCurrentUserId();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login");
            }

            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId.Value && !x.IsDeleted);
            if (user is null)
            {
                return RedirectToAction("Login");
            }

            if (!PasswordHasher.VerifyPassword(model.CurrentPassword, user.PasswordHash))
            {
                ModelState.AddModelError(nameof(model.CurrentPassword), "Mật khẩu hiện tại không đúng.");
                return View(model);
            }

            user.PasswordHash = PasswordHasher.HashPassword(model.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;
            await dbContext.SaveChangesAsync();
            await auditLogger.LogAsync("Users", "UPDATE", user.Id.ToString(), newValues: "{\"PasswordChanged\":true}", userId: user.Id);

            TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
            return RedirectToAction("ChangePassword");
        }

        private IActionResult RedirectToRoleDashboard(string? role)
        {
            return role switch
            {
                RoleNames.SuperAdmin or RoleNames.HrAdmin => RedirectToAction("Dashboard", "Admin"),
                RoleNames.Manager => RedirectToAction("DashboardDept", "Admin"),
                _ => RedirectToAction("DashboardSelf", "Admin")
            };
        }
    }
}
