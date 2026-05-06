using System.ComponentModel.DataAnnotations;

namespace QLSYLL.Models;

public class Department
{
    public int Id { get; set; }

    [Required, MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public int? ManagerId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }

    public User? Manager { get; set; }
    public ICollection<Employee> Employees { get; set; } = [];
}
