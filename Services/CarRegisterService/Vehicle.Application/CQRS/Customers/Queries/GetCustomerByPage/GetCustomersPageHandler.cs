using AutoMapper;
using BuildingBlocks.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vehicle.Application.CQRS.Customers.Queries.GetCustomersPage;
using Vehicle.Application.Data;
using Vehicle.Application.Dtos;

namespace Vehicle.Application.CQRS.Customers.Handlers
{
    public class GetCustomersPageHandler : IRequestHandler<GetCustomersPageQuery, GetCustomersPageResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCustomersPageHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetCustomersPageResult> Handle(GetCustomersPageQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Customers.AsNoTracking();

            var totalCount = await query.CountAsync(cancellationToken);

            var pageIndex = request.PaginationRequest.PageIndex < 0 ? 0 : request.PaginationRequest.PageIndex;
            var pageSize = request.PaginationRequest.PageSize <= 0 ? 10 : request.PaginationRequest.PageSize;

            var data = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var result = new PaginatedResult<CustomerDto>(
                pageIndex,
                pageSize,
                totalCount,
                _mapper.Map<List<CustomerDto>>(data)
            );

            return new GetCustomersPageResult(result);
        }
    }
}
