using BuildingBlocks.Pagination;
using PartCatalog.Application.DTOs;

namespace PartCatalog.Application.CQRS.Queries.GetPartByFilter
{
    public record GetPartByFilterQuery(
        string? Name,
        string? CateName,
        int PageIndex = 1,
        int PageSize = 10
    ) : IQuery<PaginatedResult<PartDto>>;
}
