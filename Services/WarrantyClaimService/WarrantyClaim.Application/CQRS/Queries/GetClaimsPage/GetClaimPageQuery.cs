using BuildingBlocks.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Queries.GetClaimsPage
{
    public record GetClaimsPageQuery(PaginationRequest PaginationRequest) : IQuery<GetClaimsPageResult>;

    public record GetClaimsPageResult(PaginatedResult<Claim> Result);
    
}
