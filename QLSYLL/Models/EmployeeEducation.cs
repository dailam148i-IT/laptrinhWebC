using System.ComponentModel.DataAnnotations;

namespace QLSYLL.Models;

public class EmployeeEducation
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }

    [Required, MaxLength(100)]
    public string EducationLevel { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? GeneralEducationLevel { get; set; }

    [MaxLength(200)]
    public string? SchoolName { get; set; }

    [MaxLength(150)]
    public string? Major { get; set; }

    public int? GraduationYear { get; set; }

    [MaxLength(50)]
    public string? Ranking { get; set; }

    [MaxLength(200)]
    public string? ForeignLanguage { get; set; }

    [MaxLength(200)]
    public string? ITLevel { get; set; }

    public bool IsPrimary { get; set; } = true;

    public Employee Employee { get; set; } = null!;
}
