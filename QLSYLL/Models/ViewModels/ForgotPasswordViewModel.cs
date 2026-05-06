using System.ComponentModel.DataAnnotations;

namespace QLSYLL.Models.ViewModels
{
    /// <summary>
    /// ViewModel cho form quên mật khẩu
    /// </summary>
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [Display(Name = "Email đã đăng ký")]
        public string Email { get; set; } = string.Empty;
    }
}
