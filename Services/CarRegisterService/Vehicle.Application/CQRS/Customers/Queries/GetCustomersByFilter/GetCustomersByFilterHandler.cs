using AutoMapper;
using BuildingBlocks.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vehicle.Application.Dtos;
using Vehicle.Application.Data;
using Vehicle.Domain.Models;

namespace Vehicle.Application.CQRS.Customers.Queries.GetCustomersByFilter
{
    public class GetCustomersByFilterHandler
        : IQueryHandler<GetCustomersByFilterQuery, GetCustomersByFilterResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCustomersByFilterHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetCustomersByFilterResult> Handle(GetCustomersByFilterQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Customers.AsNoTracking().AsQueryable();

            // Filter
            if (request.Filter.CustomerId.HasValue)
                query = query.Where(c => c.CustomerId == request.Filter.CustomerId.Value);

            if (!string.IsNullOrWhiteSpace(request.Filter.FullName))
                query = query.Where(c => c.FullName.Contains(request.Filter.FullName));

            if (!string.IsNullOrWhiteSpace(request.Filter.Email))
                query = query.Where(c => c.Email.Contains(request.Filter.Email));

            if (!string.IsNullOrWhiteSpace(request.Filter.Status))
                query = query.Where(c => c.Status.ToString() == request.Filter.Status);

            // Sort
            query = request.Sort.By switch
            {
                SortBy.FullName => request.Sort.Dir == SortDir.Asc ? query.OrderBy(c => c.FullName) : query.OrderByDescending(c => c.FullName),
                SortBy.Email => request.Sort.Dir == SortDir.Asc ? query.OrderBy(c => c.Email) : query.OrderByDescending(c => c.Email),
                SortBy.CreatedAt => request.Sort.Dir == SortDir.Asc ? query.OrderBy(c => c.CreatedAt) : query.OrderByDescending(c => c.CreatedAt),
                SortBy.CustomerId => request.Sort.Dir == SortDir.Asc ? query.OrderBy(c => c.CustomerId) : query.OrderByDescending(c => c.CustomerId),
                _ => query.OrderByDescending(c => c.CreatedAt),
            };

            // Pagination
            var totalCount = await query.CountAsync(cancellationToken);
            var pageIndex = request.Pagination.PageIndex < 0 ? 0 : request.Pagination.PageIndex;
            var pageSize = request.Pagination.PageSize <= 0 ? 10 : request.Pagination.PageSize;

            var data = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var paginatedResult = new PaginatedResult<CustomerDto>(
                pageIndex,
                pageSize,
                totalCount,
                _mapper.Map<List<CustomerDto>>(data)
            );

            return new GetCustomersByFilterResult(paginatedResult);
        }
    }
}
