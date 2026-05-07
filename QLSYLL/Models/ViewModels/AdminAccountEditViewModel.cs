using System.ComponentModel.DataAnnotations;

namespace QLSYLL.Models.ViewModels;

public class AdminAccountEditViewModel
{
    public int Id { get; set; }

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập họ và tên.")]
    [MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập email.")]
    [MaxLength(150)]
    [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng chọn vai trò.")]
    public string Role { get; set; } = string.Empty;
}
