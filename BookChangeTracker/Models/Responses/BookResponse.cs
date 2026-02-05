namespace BookChangeTracker.Models.Responses;

public record BookResponse(
    int Id,
    string Title,
    string Description,
    DateOnly PublishDate,
    List<AuthorResponse> Authors);
