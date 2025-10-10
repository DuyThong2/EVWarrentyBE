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
            var query = _context.Parts.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                query = query.Where(p => p.Name.Contains(request.Name));
            }

            if (!string.IsNullOrWhiteSpace(request.CateName))
            {
                query = query.Where(p => p.Category != null &&
                                         p.Category.CateName.Contains(request.CateName));
            }


            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(p => p.PartId)
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var data = items.Select(p => _mapper.Map<PartDto>(p)).ToList();

            return new PaginatedResult<PartDto>(
                request.PageIndex,
                request.PageSize,
                totalCount,
                data
            );
        }
    }
}
