using BuildingBlocks.CQRS;
using PartCatalog.Application.DTOs;

namespace PartCatalog.Application.CQRS.Queries.GetPartById
{
    public record GetPartByIdQuery(Guid Id) : IQuery<GetPartByIdResult>;

    public record GetPartByIdResult(PartDto Part);
}
