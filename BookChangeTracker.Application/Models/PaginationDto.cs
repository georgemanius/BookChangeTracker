using BookChangeTracker.Domain.Abstractions;

namespace BookChangeTracker.Application.Models;

public record PaginationDto(int PageNumber, int PageSize) : IPaginationDto;
