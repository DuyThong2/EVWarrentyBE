using AutoMapper;
using AutoMapper.QueryableExtensions;
using BuildingBlocks.CQRS;
using BuildingBlocks.Pagination;
using Microsoft.EntityFrameworkCore;
using PartCatalog.Application.Data;
using PartCatalog.Application.DTOs;

namespace PartCatalog.Application.CQRS.Queries.GetPartByFilter
{
    public class GetPartByFilterHandler
        : IQueryHandler<GetPartByFilterQuery, PaginatedResult<PartDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPartByFilterHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<PartDto>> Handle(
            GetPartByFilterQuery request,
            CancellationToken cancellationToken)
        {
            var query = _context.Parts
                .AsNoTracking();
                


            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                query = query.Where(p => p.Name.Contains(request.Name));
            }

            if (request.CateId.HasValue && request.CateId != Guid.Empty)
            {
                query = query.Where(p => p.CateId.HasValue && p.CateId == request.CateId);
            }

            if (request.PackageId.HasValue && request.PackageId != Guid.Empty)
            {
                query = query.Where(p => p.PackageId.HasValue && p.PackageId == request.PackageId);
            }

            if (!string.IsNullOrWhiteSpace(request.SerialNumber))
            {
                query = query.Where(p => p.SerialNumber != null && p.SerialNumber.Contains(request.SerialNumber));
            }

            if (!string.IsNullOrWhiteSpace(request.Manufacturer))
            {
                query = query.Where(p => p.Manufacturer != null && p.Manufacturer.Contains(request.Manufacturer));
            }


            var totalCount = await query.CountAsync(cancellationToken);

            var data = await query
                .OrderBy(p => p.PartId)
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => _mapper.Map<PartDto>(p))
                .ToListAsync(cancellationToken);

            return new PaginatedResult<PartDto>(
                request.PageIndex,
                request.PageSize,
                totalCount,
                data
            );
        }
    }
}
