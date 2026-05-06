using System.ComponentModel.DataAnnotations;

namespace QLSYLL.Models.ViewModels
{
    /// <summary>
    /// ViewModel cho form đăng nhập
    /// </summary>
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Nhớ tài khoản")]
        public bool RememberMe { get; set; }
    }
}
