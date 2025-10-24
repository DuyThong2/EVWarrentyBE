using BuildingBlocks.CQRS;
using BuildingBlocks.Pagination;
using FluentValidation;
using PartCatalog.Application.DTOs;
using PartCatalog.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartCatalog.Application.CQRS.Queries.GetWarrantyPolicyByFilter
{
    public record GetWarrantyPolicyByFilterQuery(
        string? Code,
        string? Name,
        Guid? PackageId,
        PolicyType? Type,
        ActiveStatus? Status,
        int PageIndex = 1,
        int PageSize = 10
    ) : IQuery<GetWarrantyPolicyByFilterResult>;

    public record GetWarrantyPolicyByFilterResult(PaginatedResult<WarrantyPolicyDto> Policies);

    public class GetWarrantyPolicyByFilterQueryValidator : AbstractValidator<GetWarrantyPolicyByFilterQuery>
    {
        public GetWarrantyPolicyByFilterQueryValidator()
        {
            RuleFor(x => x.PageIndex)
                .GreaterThan(0).WithMessage("PageIndex must be greater than 0");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100");
        }
    }
}
