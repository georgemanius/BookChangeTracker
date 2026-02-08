using BookChangeTracker.Domain.Abstractions;

namespace BookChangeTracker.Application.Models;

public record PaginationDto(
    int PageNumber = 1,
    int PageSize = 10) : IPaginationDto;
