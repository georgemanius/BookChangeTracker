namespace BookChangeTracker.Application.Models;

public record CreateBookDto(
    string Title,
    string Description,
    DateOnly PublishDate);
