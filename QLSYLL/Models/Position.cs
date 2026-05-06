using System.ComponentModel.DataAnnotations;

namespace QLSYLL.Models;

public class Position
{
    public int Id { get; set; }

    [Required, MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    public int Level { get; set; }
    public decimal BaseSalary { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public bool IsDeleted { get; set; }

    public ICollection<Employee> Employees { get; set; } = [];
}
