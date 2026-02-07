using System.ComponentModel.DataAnnotations;

namespace BookChangeTracker.Api.Models.Requests;

public class CreateBookRequest
{
    [Required]
    [MaxLength(200)]
    public string Title { get; init; } = string.Empty;

    [MaxLength(500)]
    public string Description { get; init; } = string.Empty;

    public DateOnly PublishDate { get; init; }
}
