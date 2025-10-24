using BuildingBlocks.Pagination;
using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using PartCatalog.Application.CQRS.Queries.GetCategoryByFilter;
using PartCatalog.Application.Data;
using PartCatalog.Application.DTOs;

namespace PartCatalog.Application.CQRS.Queries.GetCategoryByFilter
{
    public class GetCategoryByFilterHandler : IQueryHandler<GetCategoryByFilterQuery, GetCategoryByFilterResult>
    {
        private readonly IApplicationDbContext _context;

        public GetCategoryByFilterHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetCategoryByFilterResult> Handle(GetCategoryByFilterQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Categories.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.CateCode))
                query = query.Where(c => c.CateCode.Contains(request.CateCode));

            if (!string.IsNullOrWhiteSpace(request.CateName))
                query = query.Where(c => c.CateName.Contains(request.CateName));

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(c => c.CateName)
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(c => new CategoryDto
                {
                    CateId = c.CateId,
                    CateCode = c.CateCode,
                    CateName = c.CateName,
                    Description = c.Description,
                    Quantity = c.Quantity
                })
                .ToListAsync(cancellationToken);

            var paged = new PaginatedResult<CategoryDto>(
                request.PageIndex,
                request.PageSize,
                totalCount,
                items
            );

            return new GetCategoryByFilterResult(paged);
        }
    }
}
