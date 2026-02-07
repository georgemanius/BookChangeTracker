using System.ComponentModel.DataAnnotations;

namespace BookChangeTracker.Api.Models.Requests;

public class UpdateBookRequest
{
    [MaxLength(200)]
    public string? Title { get; init; }

    [MaxLength(500)]
    public string? Description { get; init; }

    public DateOnly? PublishDate { get; init; }
}
