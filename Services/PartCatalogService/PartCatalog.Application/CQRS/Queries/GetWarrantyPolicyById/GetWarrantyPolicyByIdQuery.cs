using BuildingBlocks.CQRS;
using PartCatalog.Application.DTOs;

namespace PartCatalog.Application.CQRS.Queries.GetWarrantyPolicyById
{
    public record GetWarrantyPolicyByIdQuery(Guid PolicyId)
        : IQuery<GetWarrantyPolicyByIdResult>;

    public record GetWarrantyPolicyByIdResult(WarrantyPolicyDto Policy);
}
