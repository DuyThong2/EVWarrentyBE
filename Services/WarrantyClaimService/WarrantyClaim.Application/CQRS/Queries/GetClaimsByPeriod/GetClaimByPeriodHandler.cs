using BuildingBlocks.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarrantyClaim.Application.Data;

namespace WarrantyClaim.Application.CQRS.Queries.GetClaimsByPeriod
{
    internal class GetClaimsByPeriodHandler
        : IQueryHandler<GetClaimsByPeriodQuery, GetClaimsByPeriodResult>
    {
        private readonly IApplicationDbContext _db;

        public GetClaimsByPeriodHandler(IApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<GetClaimsByPeriodResult> Handle(GetClaimsByPeriodQuery request, CancellationToken cancellationToken)
        {
            var start = request.StartDate;
            var end = request.EndDate;

            if (end < start)
                throw new ArgumentException("EndDate must be greater than or equal to StartDate.");

            var pageIndex = request.Pagination.PageIndex < 0 ? 0 : request.Pagination.PageIndex; 
            var pageSize = request.Pagination.PageSize <= 0 ? 10 : request.Pagination.PageSize;

            var baseQuery = _db.Claims
                .AsNoTracking()
                .Where(c =>
                       (c.CreatedAt >= start && c.CreatedAt <= end) ||
                       (c.LastModified >= start && c.LastModified <= end))
                .OrderByDescending(c => c.LastModified)
                .ThenByDescending(c => c.CreatedAt)
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

            return new GetClaimsByPeriodResult(page);
        }
    }
}
