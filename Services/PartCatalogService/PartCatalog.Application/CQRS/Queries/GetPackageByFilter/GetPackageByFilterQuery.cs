using BuildingBlocks.CQRS;
using BuildingBlocks.Pagination;
using FluentValidation;
using PartCatalog.Application.DTOs;

namespace PartCatalog.Application.CQRS.Queries.GetPackageByFilter
{
    public record GetPackageByFilterQuery(
        string? Name,
        string? PackageCode,
        string? Model,
        string? Status = null,
        Guid? CateId = null,
        DateTime? CreatedDate = null,
        int PageIndex = 1,
        int PageSize = 10
    ) : IQuery<GetPackageByFilterResult>;

    public record GetPackageByFilterResult(PaginatedResult<PackageDto> Packages);

    public class GetPackageByFilterQueryValidator : AbstractValidator<GetPackageByFilterQuery>
    {
        public GetPackageByFilterQueryValidator()
        {
            RuleFor(x => x.PageIndex)
                .GreaterThan(0).WithMessage("PageIndex must be greater than 0");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100");
        }
    }
}
