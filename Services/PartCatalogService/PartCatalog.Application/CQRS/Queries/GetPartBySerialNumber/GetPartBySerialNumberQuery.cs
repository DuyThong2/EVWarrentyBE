using BuildingBlocks.CQRS;
using PartCatalog.Application.DTOs;

namespace PartCatalog.Application.CQRS.Queries.GetPartBySerialNumber
{
    public record GetPartBySerialNumberQuery(string SerialNumber)
        : IQuery<GetPartBySerialNumberResult>;

    public record GetPartBySerialNumberResult(PartDto? Part);
}
