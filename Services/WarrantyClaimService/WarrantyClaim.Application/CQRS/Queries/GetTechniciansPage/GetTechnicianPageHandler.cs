using BuildingBlocks.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Queries.GetTechniciansPage
{
    
        public class GetTechniciansFilteredHandler
            : IQueryHandler<GetTechniciansFilteredQuery, GetTechniciansFilteredResult>
        {
            private readonly IApplicationDbContext _db;
            private readonly IMapper _mapper;

            public GetTechniciansFilteredHandler(IApplicationDbContext db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
            }

            public async Task<GetTechniciansFilteredResult> Handle(
                GetTechniciansFilteredQuery request,
                CancellationToken ct)
            {
                var query = _db.Technicians.AsNoTracking().AsQueryable();

                query = ApplyFilters(query, request.Filter);
                query = ApplySorting(query, request.Sort);

                var (count, data) = await ApplyPaging(query, request.Pagination, ct);

                var dtoList = MapToDto(data);

                var page = new PaginatedResult<TechnicianDto>(
                    pageIndex: request.Pagination.PageIndex,
                    pageSize: request.Pagination.PageSize,
                    count: count,
                    data: dtoList
                );

                return new GetTechniciansFilteredResult(page);
            }

            // --- Private helpers ---

            private static IQueryable<Technician> ApplyFilters(IQueryable<Technician> query, TechniciansFilter f)
            {
                if (f is null) return query;

                if (!string.IsNullOrWhiteSpace(f.FullName))
                {
                    var kw = f.FullName.Trim();
                    query = query.Where(t => t.FullName.Contains(kw));
                }

                if (!string.IsNullOrWhiteSpace(f.Email))
                {
                    var kw = f.Email.Trim();
                    query = query.Where(t => t.Email != null && t.Email.Contains(kw));
                }

                if (!string.IsNullOrWhiteSpace(f.Phone))
                {
                    var kw = f.Phone.Trim();
                    query = query.Where(t => t.Phone != null && t.Phone.Contains(kw));
                }

                if (!string.IsNullOrWhiteSpace(f.Status))
                {
                    var st = f.Status.Trim();
                    query = query.Where(t => t.Status.ToString().Equals(st, StringComparison.OrdinalIgnoreCase));
                    // if (Enum.TryParse<TechnicianStatus>(st, true, out var parsed))
                    //     query = query.Where(t => t.Status == parsed);
                }

                return query;
            }

            private static IQueryable<Technician> ApplySorting(IQueryable<Technician> query, SortOption sort)
            {
                return sort switch
                {
                    { By: TechSortBy.FullName, Dir: TechSortDir.Asc } => query.OrderBy(t => t.FullName).ThenBy(t => t.Id),
                    { By: TechSortBy.FullName, Dir: TechSortDir.Desc } => query.OrderByDescending(t => t.FullName).ThenByDescending(t => t.Id),

                    { By: TechSortBy.CreatedAt, Dir: TechSortDir.Asc } => query.OrderBy(t => t.CreatedAt).ThenBy(t => t.Id),
                    { By: TechSortBy.CreatedAt, Dir: TechSortDir.Desc } => query.OrderByDescending(t => t.CreatedAt).ThenByDescending(t => t.Id),

                    { By: TechSortBy.LastModified, Dir: TechSortDir.Asc } => query.OrderBy(t => t.LastModified).ThenBy(t => t.Id),
                    { By: TechSortBy.LastModified, Dir: TechSortDir.Desc } => query.OrderByDescending(t => t.LastModified).ThenByDescending(t => t.Id),

                    { By: TechSortBy.Id, Dir: TechSortDir.Asc } => query.OrderBy(t => t.Id),
                    { By: TechSortBy.Id, Dir: TechSortDir.Desc } => query.OrderByDescending(t => t.Id),

                    _ => query.OrderBy(t => t.FullName).ThenBy(t => t.Id)
                };
            }

            private static async Task<(int count, List<Technician> data)> ApplyPaging(
                IQueryable<Technician> query,
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

            private List<TechnicianDto> MapToDto(List<Technician> data)
            {
                return data.Select(_mapper.Map<TechnicianDto>).ToList();
            }
        }
    }

