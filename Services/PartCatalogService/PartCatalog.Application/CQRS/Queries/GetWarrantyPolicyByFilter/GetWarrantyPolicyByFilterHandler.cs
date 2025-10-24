using BuildingBlocks.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PartCatalog.Application.CQRS.Queries.GetWarrantyPolicyByFilter;
using PartCatalog.Application.Data;
using PartCatalog.Application.DTOs;
using PartCatalog.Domain.Enums;

namespace PartCatalog.Application.CQRS.Queries.GetWarrantyPolicyByFilter
{
    public class GetWarrantyPolicyByFilterHandler : IRequestHandler<GetWarrantyPolicyByFilterQuery, GetWarrantyPolicyByFilterResult>
    {
        private readonly IApplicationDbContext _context;

        public GetWarrantyPolicyByFilterHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetWarrantyPolicyByFilterResult> Handle(GetWarrantyPolicyByFilterQuery request, CancellationToken cancellationToken)
        {
            var query = _context.WarrantyPolicies
                .Include(w => w.Package)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Code))
                query = query.Where(w => w.Code != null && w.Code.Contains(request.Code));

            if (!string.IsNullOrWhiteSpace(request.Name))
                query = query.Where(w => w.Name.Contains(request.Name));

            if (request.PackageId.HasValue && request.PackageId != Guid.Empty)
                query = query.Where(w => w.PackageId == request.PackageId.Value);

            if (request.Type.HasValue)
                query = query.Where(w => w.Type.HasValue && w.Type == request.Type.Value);

            if (request.Status.HasValue)
                query = query.Where(w => w.Status.HasValue && w.Status == request.Status.Value);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(w => w.CreatedAt)
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(w => new WarrantyPolicyDto
                {
                    PolicyId = w.PolicyId,
                    PackageId = w.PackageId,
                    Code = w.Code,
                    Name = w.Name,
                    Type = w.Type,
                    Status = w.Status,
                    Description = w.Description,
                    WarrantyDuration = w.WarrantyDuration,
                    WarrantyDistance = w.WarrantyDistance,
                    CreatedAt = w.CreatedAt,
                    UpdatedAt = w.UpdatedAt,
                    PackageName = w.Package != null ? w.Package.Name : null
                })
                .ToListAsync(cancellationToken);

            var pagedResult = new PaginatedResult<WarrantyPolicyDto>(
                request.PageIndex,
                request.PageSize,
                totalCount,
                items
            );

            return new GetWarrantyPolicyByFilterResult(pagedResult);
        }
    }
}
