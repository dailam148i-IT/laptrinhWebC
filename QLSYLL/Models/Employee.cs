using System.ComponentModel.DataAnnotations;

namespace QLSYLL.Models;

public class Employee
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required, MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    public DateOnly BirthDate { get; set; }

    [MaxLength(20)]
    public string Gender { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? AliasName { get; set; }

    [MaxLength(200)]
    public string? BirthPlace { get; set; }

    [MaxLength(300)]
    public string? Hometown { get; set; }

    [MaxLength(300)]
    public string? PermanentAddress { get; set; }

    [MaxLength(30)]
    public string Phone { get; set; } = string.Empty;

    [MaxLength(150), EmailAddress]
    public string? PersonalEmail { get; set; }

    [MaxLength(150), EmailAddress]
    public string? CompanyEmail { get; set; }

    [MaxLength(30)]
    public string? IdentityNumber { get; set; }

    public DateOnly? IdentityIssuedDate { get; set; }

    [MaxLength(200)]
    public string? IdentityIssuedPlace { get; set; }

    [MaxLength(300)]
    public string? CurrentAddress { get; set; }

    [MaxLength(50)]
    public string? MaritalStatus { get; set; }

    [MaxLength(100)]
    public string? Ethnicity { get; set; }

    [MaxLength(100)]
    public string? Religion { get; set; }

    [MaxLength(50)]
    public string? TaxCode { get; set; }

    [MaxLength(50)]
    public string? SocialInsuranceNumber { get; set; }

    [MaxLength(50)]
    public string? BankAccountNumber { get; set; }

    [MaxLength(150)]
    public string? BankName { get; set; }

    [MaxLength(150)]
    public string? BankBranch { get; set; }

    public int DepartmentId { get; set; }
    public int PositionId { get; set; }
    public DateOnly JoinDate { get; set; }

    [MaxLength(255)]
    public string? AvatarPath { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = "Đã duyệt";

    public decimal Salary { get; set; }

    [MaxLength(200)]
    public string? FamilyInfo { get; set; }

    public DateOnly? YouthUnionJoinDate { get; set; }

    [MaxLength(200)]
    public string? YouthUnionJoinPlace { get; set; }

    public DateOnly? CommunistPartyJoinDate { get; set; }

    [MaxLength(200)]
    public string? CommunistPartyJoinPlace { get; set; }

    [MaxLength(50)]
    public string? CommunistPartyStatus { get; set; }

    public bool IsDeleted { get; set; }
    public int? DeletedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public User User { get; set; } = null!;
    public Department Department { get; set; } = null!;
    public Position Position { get; set; } = null!;
    public ICollection<EmployeeSkill> EmployeeSkills { get; set; } = [];
    public ICollection<EmployeeEducation> EmployeeEducations { get; set; } = [];
    public ICollection<WorkHistory> WorkHistories { get; set; } = [];
    public ICollection<FamilyMember> FamilyMembers { get; set; } = [];
    public ICollection<EmployeeDocument> EmployeeDocuments { get; set; } = [];
}
