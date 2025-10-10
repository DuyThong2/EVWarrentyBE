using AutoMapper;
using BuildingBlocks.Pagination;
using BuildingBlocks.Exceptions;
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
            try
            {
                var query = _context.Customers.AsNoTracking().AsQueryable();

                // Filter
                if (request.Filter.CustomerId.HasValue)
                    query = query.Where(c => c.CustomerId == request.Filter.CustomerId.Value);

                if (!string.IsNullOrWhiteSpace(request.Filter.FullName))
                    query = query.Where(c => c.FullName.Contains(request.Filter.FullName));

                if (!string.IsNullOrWhiteSpace(request.Filter.Email))
                    query = query.Where(c => c.Email.Contains(request.Filter.Email));

                if (!string.IsNullOrWhiteSpace(request.Filter.PhoneNumber))
                    query = query.Where(c => c.PhoneNumber.Contains(request.Filter.PhoneNumber));

                if (!string.IsNullOrWhiteSpace(request.Filter.Address))
                    query = query.Where(c => c.Address.Contains(request.Filter.Address));

                if (!string.IsNullOrWhiteSpace(request.Filter.Status))
                {
                    // Try to parse status as enum, if failed, compare as string
                    if (System.Enum.TryParse<Vehicle.Domain.Enums.CustomerStatus>(request.Filter.Status, true, out var statusEnum))
                    {
                        query = query.Where(c => c.Status == statusEnum);
                    }
                    else
                    {
                        // Fallback to string comparison for backward compatibility
                        query = query.Where(c => c.Status.ToString().Equals(request.Filter.Status, StringComparison.OrdinalIgnoreCase));
                    }
                }

                // Sort
                query = request.Sort.By switch
                {
                    SortBy.FullName => request.Sort.Dir == SortDir.Asc ? query.OrderBy(c => c.FullName) : query.OrderByDescending(c => c.FullName),
                    SortBy.Email => request.Sort.Dir == SortDir.Asc ? query.OrderBy(c => c.Email) : query.OrderByDescending(c => c.Email),
                    SortBy.PhoneNumber => request.Sort.Dir == SortDir.Asc ? query.OrderBy(c => c.PhoneNumber) : query.OrderByDescending(c => c.PhoneNumber),
                    SortBy.Address => request.Sort.Dir == SortDir.Asc ? query.OrderBy(c => c.Address) : query.OrderByDescending(c => c.Address),
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
            catch (Exception ex)
            {
                throw new InternalServerException($"An error occurred while filtering customers: {ex.Message}", ex.StackTrace ?? string.Empty);
            }
        }
    }
}
