namespace BookChangeTracker.Domain.Abstractions;

public interface IPaginationDto
{
    int PageNumber { get; }
    int PageSize { get; }
}
