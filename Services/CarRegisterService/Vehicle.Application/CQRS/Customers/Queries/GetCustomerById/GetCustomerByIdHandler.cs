using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Vehicle.Application.CQRS.Customers.Queries.GetCustomerById;

using Vehicle.Application.Data;
using Vehicle.Application.Dtos;

namespace Vehicle.Application.Queries
{
    public class GetCustomerByIdHandler : IQueryHandler<GetCustomerByIdQuery, GetCustomerByIdResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCustomerByIdHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetCustomerByIdResult> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers
                .Include(c => c.Vehicles)
                    .ThenInclude(v => v.Parts)
                .FirstOrDefaultAsync(c => c.CustomerId == request.CustomerId, cancellationToken);

            if (customer == null)
                throw new KeyNotFoundException($"Customer {request.CustomerId} not found.");

            var dto = _mapper.Map<CustomerDto>(customer);

            return new GetCustomerByIdResult(dto);
        }
    }
}
