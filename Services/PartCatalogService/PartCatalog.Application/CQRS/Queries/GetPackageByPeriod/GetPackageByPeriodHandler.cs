using BuildingBlocks.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PartCatalog.Application.Data;
using PartCatalog.Application.DTOs;

namespace PartCatalog.Application.CQRS.Queries.GetPackageByPeriod
{
    public class GetPackageByPeriodHandler : IRequestHandler<GetPackageByPeriodQuery, GetPackageByPeriodResult>
    {
        private readonly IApplicationDbContext _context;

        public GetPackageByPeriodHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetPackageByPeriodResult> Handle(GetPackageByPeriodQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Packages
                .Where(p => p.CreatedAt >= request.StartDate && p.CreatedAt <= request.EndDate)
                .OrderByDescending(p => p.CreatedAt)
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
                    UpdatedAt = p.UpdatedAt
                });

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var pagedResult = new PaginatedResult<PackageDto>(
                request.PageIndex,
                request.PageSize,
                totalCount,
                items
            );

            return new GetPackageByPeriodResult(pagedResult);
        }
    }

    public record GetPackageByPeriodResult(PaginatedResult<PackageDto> Packages);
}
