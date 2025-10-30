using BuildingBlocks.CQRS;
using PartCatalog.Domain.Models;

namespace PartCatalog.Application.CQRS.Queries.GetPackageById
{
    public record GetPackageByIdQuery(Guid PackageId) : IQuery<GetPackageByIdResult>;

    public record GetPackageByIdResult(Package Package);
}
