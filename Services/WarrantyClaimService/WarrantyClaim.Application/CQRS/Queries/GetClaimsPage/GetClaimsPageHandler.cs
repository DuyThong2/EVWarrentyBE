using BuildingBlocks.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarrantyClaim.Application.Data;

namespace WarrantyClaim.Application.CQRS.Queries.GetClaimsPage
{
    public class GetClaimsPageHandler : IQueryHandler<GetClaimsPageQuery, GetClaimsPageResult>
    {
        private readonly IApplicationDbContext _db;

        public GetClaimsPageHandler(IApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<GetClaimsPageResult> Handle(GetClaimsPageQuery request, CancellationToken cancellationToken)
        {
            // Ensure valid inputs (zero-based pageIndex)
            var pageIndex = request.PaginationRequest.PageIndex < 0 ? 0 : request.PaginationRequest.PageIndex;
            var pageSize = request.PaginationRequest.PageSize <= 0 ? 10 : request.PaginationRequest.PageSize;

            var baseQuery = _db.Claims
                .AsNoTracking()
                .OrderByDescending(c => c.CreatedAt)  
                .ThenByDescending(c => c.Id);        

            var totalCount = await baseQuery.CountAsync(cancellationToken);

            var data = await baseQuery
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var page = new PaginatedResult<Claim>(
                pageIndex: pageIndex,
                pageSize: pageSize,
                count: totalCount,
                data: data
            );

            return new GetClaimsPageResult(page);
        }
    }
}

