using BuildingBlocks.Pagination;
using PartCatalog.Application.DTOs;

namespace PartCatalog.Application.CQRS.Queries.GetAllParts
{
    public record GetAllPartsQuery(int PageIndex = 1, int PageSize = 10)
        : IQuery<PaginatedResult<PartDto>>;

    public record GetAllPartsResult(IReadOnlyList<PartDto> Parts, int TotalCount);
}
