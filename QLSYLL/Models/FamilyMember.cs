using System.ComponentModel.DataAnnotations;

namespace QLSYLL.Models;

public class FamilyMember
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }

    [Required, MaxLength(50)]
    public string Relationship { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    public int? BirthYear { get; set; }

    [MaxLength(150)]
    public string? Occupation { get; set; }

    [MaxLength(200)]
    public string? Workplace { get; set; }

    [MaxLength(30)]
    public string? PhoneNumber { get; set; }

    public bool IsEmergencyContact { get; set; }

    public Employee Employee { get; set; } = null!;
}
