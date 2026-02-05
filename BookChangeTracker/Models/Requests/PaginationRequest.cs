using System.ComponentModel.DataAnnotations;

namespace BookChangeTracker.Models.Requests;

public record PaginationRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "Page number must be at least 1")]
    public int PageNumber { get; init; } = 1;

    [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
    public int PageSize { get; init; } = 10;
}
