using BuildingBlocks.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Queries.GetClaimsByPeriod
{
    public record GetClaimsByPeriodQuery(
        DateTime StartDate,
        DateTime EndDate,
        PaginationRequest Pagination
    ) : IQuery<GetClaimsByPeriodResult>;

    public record GetClaimsByPeriodResult(PaginatedResult<Claim> Result);
}
