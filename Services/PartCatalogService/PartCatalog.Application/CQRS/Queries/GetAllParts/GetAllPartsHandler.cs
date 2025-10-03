using BuildingBlocks.Pagination;
using PartCatalog.Application.CQRS.Queries.GetAllParts;
using PartCatalog.Application.DTOs;
using PartCatalog.Application.Data;
using BuildingBlocks.CQRS;
using AutoMapper;

public class GetAllPartsHandler
    : IQueryHandler<GetAllPartsQuery, PaginatedResult<PartDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllPartsHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<PartDto>> Handle(GetAllPartsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Parts.AsNoTracking();

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(p => p.PartId)
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var data = items.Select(p => _mapper.Map<PartDto>(p)).ToList();

        return new PaginatedResult<PartDto>(request.PageIndex, request.PageSize, totalCount, data);
    }
}
