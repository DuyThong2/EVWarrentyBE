using AutoMapper;
using BuildingBlocks.Pagination;
using WarrantyClaim.Application.CQRS.Queries.GetClaimByFilter;
using WarrantyClaim.Application.Data;
using WarrantyClaim.Application.Extension;


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
        if (f is null) return query;

        if (f.Id is Guid id)
            query = query.Where(c => c.Id == id);

        if (!string.IsNullOrWhiteSpace(f.VIN))
            query = query.Where(c => c.VIN.Contains(f.VIN));

        if (EnumParser.TryParseEnum<ClaimStatus>(f.Status, out var parsedStatus))
            query = query.Where(c => c.Status == parsedStatus);

        if (EnumParser.TryParseEnum<ClaimType>(f.ClaimType, out var parsedType))
            query = query.Where(c => c.ClaimType == parsedType);

        // --- Time range (theo dateField) ---
        if (f.Start.HasValue || f.End.HasValue)
        {
            var start = f.Start ?? DateTime.MinValue;
            var end = f.End ?? DateTime.MaxValue;

            // dateField: createdAt | lastModified | null => mặc định createdAt
            var df = (f.DateField ?? "createdAt").Trim();
            if (df.Equals("lastModified", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(c => c.LastModified >= start && c.LastModified <= end);
            }
            else if (df.Equals("createdAt", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(c => c.CreatedAt >= start && c.CreatedAt <= end);
            }
            else
            {
                // fallback: áp cho cả hai nếu dateField không hợp lệ
                query = query.Where(c =>
                    (c.CreatedAt >= start && c.CreatedAt <= end) ||
                    (c.LastModified >= start && c.LastModified <= end));
            }
        }

        // --- Distance range: UI gửi km -> convert sang mét ---
        int? minM = null, maxM = null;
        if (f.DistanceMin.HasValue) minM = (int)Math.Round(f.DistanceMin.Value);
        if (f.DistanceMax.HasValue) maxM = (int)Math.Round(f.DistanceMax.Value);

        if (minM.HasValue) query = query.Where(c => c.DistanceMeter >= minM.Value);
        if (maxM.HasValue) query = query.Where(c => c.DistanceMeter <= maxM.Value);

        // --- Total price range ---
        if (f.PriceMin.HasValue) query = query.Where(c => c.TotalPrice >= f.PriceMin.Value);
        if (f.PriceMax.HasValue) query = query.Where(c => c.TotalPrice <= f.PriceMax.Value);

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
