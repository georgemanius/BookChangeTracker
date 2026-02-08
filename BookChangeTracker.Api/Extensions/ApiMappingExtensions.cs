using BookChangeTracker.Api.Models.Requests;
using BookChangeTracker.Api.Models.Responses;
using BookChangeTracker.Application.Models;

namespace BookChangeTracker.Api.Extensions;

public static class ApiMappingExtensions
{
    public static CreateBookDto ToCreateBookDto(this CreateBookRequest request) =>
        new(
            request.Title,
            request.Description,
            request.PublishDate);

    public static UpdateBookDto ToUpdateBookDto(this UpdateBookRequest request) =>
        new(
            request.Title,
            request.Description,
            request.PublishDate);

    public static CreateAuthorDto ToCreateAuthorDto(this CreateAuthorRequest request) => new(request.Name);

    public static ChangeLogFilterDto ToChangeLogFilterDto(this ChangeLogFilterRequest request) =>
        new(
            request.FieldName,
            request.FromDate,
            request.ToDate,
            request.ChangedBy,
            request.OrderBy,
            request.SortOrder);

    public static PaginationDto ToPaginationDto(this PaginationRequest request)
    {
        const int DefaultPageNumber = 1;
        const int DefaultPageSize = 20;
        
        return new(
            request.PageNumber ?? DefaultPageNumber,
            request.PageSize ?? DefaultPageSize);
    }

    public static BookResponse ToResponse(this BookDto dto) =>
        new(
            dto.Id,
            dto.Title,
            dto.Description,
            dto.PublishDate,
            dto.Authors.Select(a => a.ToResponse()).ToList());

    public static AuthorResponse ToResponse(this AuthorDto dto) => new(dto.Id, dto.Name);

    public static BookChangeLogResponse ToResponse(this BookChangeLogDto dto) =>
        new(
            dto.Id,
            dto.BookId,
            dto.FieldName,
            dto.OldValue,
            dto.NewValue,
            dto.ChangedAt,
            dto.Description,
            dto.ChangedBy,
            dto.ChangeType,
            dto.TargetAuthorId);
}
