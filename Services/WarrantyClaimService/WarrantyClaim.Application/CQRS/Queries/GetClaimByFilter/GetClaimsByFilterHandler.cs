using AutoMapper;
using BuildingBlocks.Pagination;
using WarrantyClaim.Application.CQRS.Queries.GetClaimByFilter;
using WarrantyClaim.Application.Data;


namespace WarrantyClaim.Application.CQRS.Queries.GetClaimByFilter;
internal class GetClaimsFilteredHandler
    : IQueryHandler<GetClaimsFilteredQuery, GetClaimsFilteredResult>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetClaimsFilteredHandler(IApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<GetClaimsFilteredResult> Handle(GetClaimsFilteredQuery request, CancellationToken ct)
    {
        var query = _db.Claims.AsNoTracking().AsQueryable();

        query = ApplyFilters(query, request.Filter);
        query = ApplyIncludes(query, request.Include);
        query = ApplySorting(query, request.Sort);

        var (count, data) = await ApplyPaging(query, request.Pagination, ct);

        var dtoList = MapToDto(data);

        var page = new PaginatedResult<ClaimDto>(
            request.Pagination.PageIndex,
            request.Pagination.PageSize,
            count,
            dtoList
        );

        return new GetClaimsFilteredResult(page);
    }

    // --- Private helpers ---

    private IQueryable<Claim> ApplyFilters(IQueryable<Claim> query, ClaimsFilter f)
    {
        if (f.Id is Guid id) query = query.Where(c => c.Id == id);
        if (f.StaffId is Guid staffId) query = query.Where(c => c.StaffId == staffId);
        if (!string.IsNullOrWhiteSpace(f.VIN))
            query = query.Where(c => c.VIN.Contains(f.VIN));
        if (!string.IsNullOrWhiteSpace(f.Status))
            query = query.Where(c => c.Status.ToString() == f.Status);
        if (!string.IsNullOrWhiteSpace(f.ClaimType))
            query = query.Where(c => c.ClaimType.ToString() == f.ClaimType);
        if (f.ClaimItemId is Guid claimItemId)
            query = query.Where(c => c.Items.Any(i => i.Id == claimItemId));
        if (f.Start.HasValue && f.End.HasValue)
        {
            var start = f.Start.Value;
            var end = f.End.Value;
            query = query.Where(c =>
                (c.CreatedAt >= start && c.CreatedAt <= end) ||
                (c.LastModified >= start && c.LastModified <= end));
        }
        return query;
    }

    private IQueryable<Claim> ApplyIncludes(IQueryable<Claim> query, IncludeOption include)
    {
        if (include.Technician)
            query = query.Include(c => c.Technician);

        if (include.Items)
            query = query.Include(c => c.Items);

        if (include.WorkOrdersWithTech)
        {
            query = query.Include(c => c.Items)
                         .ThenInclude(i => i.WorkOrders)
                         .ThenInclude(w => w.Technician);
        }

        return query;
    }

    private IQueryable<Claim> ApplySorting(IQueryable<Claim> query, SortOption sort)
    {
        return sort switch
        {
            { By: SortBy.TotalPrice, Dir: SortDir.Desc }
                => query.OrderByDescending(c => c.TotalPrice).ThenByDescending(c => c.Id),

            { By: SortBy.TotalPrice, Dir: SortDir.Asc }
                => query.OrderBy(c => c.TotalPrice).ThenBy(c => c.Id),

            { By: SortBy.CreatedAt, Dir: SortDir.Desc }
                => query.OrderByDescending(c => c.CreatedAt).ThenByDescending(c => c.Id),

            { By: SortBy.CreatedAt, Dir: SortDir.Asc }
                => query.OrderBy(c => c.CreatedAt).ThenBy(c => c.Id),

            { By: SortBy.LastModified, Dir: SortDir.Asc }
                => query.OrderBy(c => c.LastModified).ThenBy(c => c.Id),

            _ // default LastModified Desc
                => query.OrderByDescending(c => c.LastModified).ThenByDescending(c => c.Id),
        };
    }

    private async Task<(int count, List<Claim> data)> ApplyPaging(
        IQueryable<Claim> query,
        PaginationRequest pagination,
        CancellationToken ct)
    {
        var pageIndex = pagination.PageIndex < 0 ? 0 : pagination.PageIndex;
        var pageSize = pagination.PageSize <= 0 ? 10 : pagination.PageSize;

        var count = await query.CountAsync(ct);
        var data = await query
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (count, data);
    }

    private List<ClaimDto> MapToDto(List<Claim> data)
    {
        return data.Select(_mapper.Map<ClaimDto>).ToList();
    }
}
