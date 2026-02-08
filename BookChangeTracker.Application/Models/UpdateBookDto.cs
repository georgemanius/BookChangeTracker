namespace BookChangeTracker.Application.Models;

public record UpdateBookDto(
    string? Title,
    string? Description,
    DateOnly? PublishDate);
