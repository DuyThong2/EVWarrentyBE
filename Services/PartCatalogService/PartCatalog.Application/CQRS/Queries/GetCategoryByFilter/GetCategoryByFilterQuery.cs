using BuildingBlocks.CQRS;
using BuildingBlocks.Pagination;
using FluentValidation;
using PartCatalog.Application.DTOs;

namespace PartCatalog.Application.CQRS.Queries.GetCategoryByFilter
{
    public record GetCategoryByFilterQuery(
        string? CateCode,
        string? CateName,
        int PageIndex = 1,
        int PageSize = 10
    ) : IQuery<GetCategoryByFilterResult>;

    public record GetCategoryByFilterResult(PaginatedResult<CategoryDto> Categories);

    public class GetCategoryByFilterQueryValidator : AbstractValidator<GetCategoryByFilterQuery>
    {
        public GetCategoryByFilterQueryValidator()
        {
            RuleFor(x => x.PageIndex)
                .GreaterThan(0).WithMessage("PageIndex must be greater than 0");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100");
        }
    }
}
