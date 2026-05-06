using System.ComponentModel.DataAnnotations;

namespace QLSYLL.Models;

public class Role
{
    public int Id { get; set; }

    [Required, MaxLength(30)]
    public string Code { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public ICollection<User> Users { get; set; } = [];
}
