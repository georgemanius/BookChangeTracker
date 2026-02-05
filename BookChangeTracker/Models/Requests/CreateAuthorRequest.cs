using System.ComponentModel.DataAnnotations;

namespace BookChangeTracker.Models.Requests;

public class CreateAuthorRequest
{
    [Required]
    [MaxLength(100)]
    public string Name { get; init; } = string.Empty;
}
