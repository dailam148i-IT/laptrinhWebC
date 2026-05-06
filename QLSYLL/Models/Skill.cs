using System.ComponentModel.DataAnnotations;

namespace QLSYLL.Models;

public class Skill
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public ICollection<EmployeeSkill> EmployeeSkills { get; set; } = [];
}
