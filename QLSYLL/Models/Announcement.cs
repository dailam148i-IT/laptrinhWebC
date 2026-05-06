using System.ComponentModel.DataAnnotations;

namespace QLSYLL.Models;

public class Announcement
{
    public int Id { get; set; }

    [Required, MaxLength(250)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string Priority { get; set; } = "Bình thường";

    [Required, MaxLength(50)]
    public string Status { get; set; } = "Bản nháp";

    public int AuthorUserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public User AuthorUser { get; set; } = null!;
}
