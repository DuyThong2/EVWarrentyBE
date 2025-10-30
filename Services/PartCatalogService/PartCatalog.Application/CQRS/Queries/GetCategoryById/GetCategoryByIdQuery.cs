using BuildingBlocks.CQRS;

namespace PartCatalog.Application.CQRS.Queries.GetCategoryById
{
    public record GetCategoryByIdQuery(Guid CateId)
        : IQuery<GetCategoryByIdResult>;
}
