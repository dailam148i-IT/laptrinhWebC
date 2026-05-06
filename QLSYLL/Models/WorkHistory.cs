using System.ComponentModel.DataAnnotations;

namespace QLSYLL.Models;

public class WorkHistory
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }

    [Required, MaxLength(150)]
    public string CompanyName { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string PositionTitle { get; set; } = string.Empty;

    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public Employee Employee { get; set; } = null!;
}
