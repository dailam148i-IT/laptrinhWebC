using System.ComponentModel.DataAnnotations;

namespace QLSYLL.Models;

public class Resume
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required, MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    public DateOnly BirthDate { get; set; }

    [MaxLength(20)]
    public string Gender { get; set; } = string.Empty;

    [MaxLength(30)]
    public string Phone { get; set; } = string.Empty;

    [MaxLength(150), EmailAddress]
    public string? PersonalEmail { get; set; }

    [MaxLength(30)]
    public string? IdentityNumber { get; set; }

    [MaxLength(300)]
    public string? Address { get; set; }

    public int DepartmentId { get; set; }
    public int PositionId { get; set; }
    public DateOnly JoinDate { get; set; }

    [MaxLength(100)]
    public string? EducationLevel { get; set; }

    [MaxLength(200)]
    public string? SchoolName { get; set; }

    [MaxLength(150)]
    public string? Major { get; set; }

    public int? GraduationYear { get; set; }

    [MaxLength(500)]
    public string? Skills { get; set; }

    [MaxLength(255)]
    public string? AvatarPath { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = "Đã duyệt";

    public decimal Salary { get; set; }

    [MaxLength(200)]
    public string? FamilyInfo { get; set; }

    public bool IsDeleted { get; set; }
    public int? DeletedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public User User { get; set; } = null!;
    public Department Department { get; set; } = null!;
    public Position Position { get; set; } = null!;
}
