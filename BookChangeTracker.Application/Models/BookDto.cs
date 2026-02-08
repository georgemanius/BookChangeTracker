namespace BookChangeTracker.Application.Models;

public record BookDto(
    int Id,
    string Title,
    string Description,
    DateOnly PublishDate,
    List<AuthorDto> Authors);
