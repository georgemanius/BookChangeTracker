using BookChangeTracker.Application.Abstractions;
using BookChangeTracker.Application.Extensions;
using BookChangeTracker.Application.Models;
using BookChangeTracker.Infrastructure.Abstractions;
using BookChangeTracker.Infrastructure.Models.Enums;
using BookChangeTracker.Infrastructure.Repositories;

namespace BookChangeTracker.Application.Services;


public class ChangeLogService(IChangeLogRepository changeLogRepository) : IChangeLogService
{
    public async Task<(List<BookChangeLogDto> Logs, int TotalCount)> GetBookChangeLogsAsync(
        int bookId,
        ChangeLogFilterDto filter,
        PaginationDto pagination)
    {
        var filterWrapper = new FilterDtoWrapper(filter);
        var paginationWrapper = new PaginationDtoWrapper(pagination);
        
        var (logs, totalCount) = await changeLogRepository.GetByBookIdAsync(bookId, filterWrapper, paginationWrapper);
        return (logs.Select(l => l.ToBookChangeLogDto()).ToList(), totalCount);
    }

    private class FilterDtoWrapper : IFilterDto
    {
        private readonly ChangeLogFilterDto _dto;

        public FilterDtoWrapper(ChangeLogFilterDto dto)
        {
            _dto = dto;
        }

        public string? FieldName => _dto.FieldName;
        public DateTime? FromDate => _dto.FromDate;
        public DateTime? ToDate => _dto.ToDate;
        public string? ChangedBy => _dto.ChangedBy;
        public ChangeLogSortFields OrderBy => Enum.Parse<ChangeLogSortFields>(_dto.OrderBy);
        public SortOrder SortOrder => Enum.Parse<SortOrder>(_dto.SortOrder);
        public int PageNumber => 1;
        public int PageSize => 10;
    }

    private class PaginationDtoWrapper : IPaginationDto
    {
        private readonly PaginationDto _dto;

        public PaginationDtoWrapper(PaginationDto dto)
        {
            _dto = dto;
        }

        public int PageNumber => _dto.PageNumber;
        public int PageSize => _dto.PageSize;
    }
}
