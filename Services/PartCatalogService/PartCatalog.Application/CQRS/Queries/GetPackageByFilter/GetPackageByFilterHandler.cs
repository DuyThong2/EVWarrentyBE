using BuildingBlocks.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PartCatalog.Application.CQRS.Queries.GetPackageByFilter;
using PartCatalog.Application.Data;
using PartCatalog.Application.DTOs;

namespace PartCatalog.Application.CQRS.Queries.GetPackageByFilter
{
    public class GetPackageByFilterHandler : IRequestHandler<GetPackageByFilterQuery, GetPackageByFilterResult>
    {
        private readonly IApplicationDbContext _context;

        public GetPackageByFilterHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetPackageByFilterResult> Handle(GetPackageByFilterQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Packages
                .Include(p => p.Category)
                .Include(p => p.Parts)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Name))
                query = query.Where(p => p.Name.Contains(request.Name));

            if (!string.IsNullOrWhiteSpace(request.PackageCode))
                query = query.Where(p => p.PackageCode.Contains(request.PackageCode));

            if (!string.IsNullOrWhiteSpace(request.Model))
                query = query.Where(p => p.Model != null && p.Model.Contains(request.Model));

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new PackageDto
                {
                    PackageId = p.PackageId,
                    PackageCode = p.PackageCode,
                    Name = p.Name,
                    Description = p.Description,
                    Model = p.Model,
                    Status = p.Status.HasValue ? p.Status.Value.ToString() : null,
                    Quantity = p.Quantity,
                    Note = p.Note,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    CategoryName = p.Category != null ? p.Category.CateName : null
                })
                .ToListAsync(cancellationToken);

            var pagedResult = new PaginatedResult<PackageDto>(
                request.PageIndex,
                request.PageSize,
                totalCount,
                items
            );

            return new GetPackageByFilterResult(pagedResult);
        }
    }
}
