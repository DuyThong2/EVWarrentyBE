using BuildingBlocks.Pagination;
using PartCatalog.Application.DTOs;
using System;

namespace PartCatalog.Application.CQRS.Queries.GetPartByFilter
{
    public record GetPartByFilterQuery(
        string? Name,
        Guid? CateId,
        Guid? PackageId,
        string? SerialNumber,
        string? Manufacturer,
        int PageIndex = 1,
        int PageSize = 10
    ) : IQuery<PaginatedResult<PartDto>>;
}
