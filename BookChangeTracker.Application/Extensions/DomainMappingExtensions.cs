using BookChangeTracker.Application.Models;
using BookChangeTracker.Domain.Models.Entities;

namespace BookChangeTracker.Application.Extensions;

public static class DomainMappingExtensions
{
    public static Book ToBook(this CreateBookDto dto) =>
        new()
        {
            Title = dto.Title,
            Description = dto.Description,
            PublishDate = dto.PublishDate
        };

    public static BookDto ToBookDto(this Book book) =>
        new(
            book.Id,
            book.Title,
            book.Description,
            book.PublishDate,
            book.BookAuthors.Select(ba => ba.Author.ToAuthorDto()).ToList());

    public static void ApplyToBook(this UpdateBookDto dto, Book book)
    {
        if (dto.Title is not null)
            book.Title = dto.Title;

        if (dto.Description is not null)
            book.Description = dto.Description;

        if (dto.PublishDate.HasValue)
            book.PublishDate = dto.PublishDate.Value;
    }

    public static Author ToAuthor(this CreateAuthorDto dto) =>
        new()
        {
            Name = dto.Name
        };

    public static AuthorDto ToAuthorDto(this Author author) => new(author.Id, author.Name);

    public static BookChangeLogDto ToBookChangeLogDto(this BookChangeLog log) =>
        new(
            log.Id,
            log.BookId,
            log.FieldName,
            log.OldValue,
            log.NewValue,
            log.ChangedAt,
            log.Description,
            log.ChangedBy,
            log.ChangeType.ToString(),
            log.TargetAuthorId);
}
