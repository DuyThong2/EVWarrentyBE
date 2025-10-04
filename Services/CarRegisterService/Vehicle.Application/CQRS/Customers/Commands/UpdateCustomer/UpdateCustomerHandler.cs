using AutoMapper;
using Microsoft.EntityFrameworkCore;

using Vehicle.Application.CQRS.Customers.Commands.UpdateCustomer;
using Vehicle.Application.Data;

namespace Vehicle.Application.Commands
{
    public class UpdateCustomerHandler : ICommandHandler<UpdateCustomerCommand, UpdateCustomerResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateCustomerHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UpdateCustomerResult> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers
                .Include(c => c.Vehicles)
                    .ThenInclude(v => v.Parts)
                .FirstOrDefaultAsync(c => c.CustomerId == request.Customer.CustomerId, cancellationToken);

            if (customer == null)
                throw new KeyNotFoundException($"Customer {request.Customer.CustomerId} not found.");

            _mapper.Map(request.Customer, customer);

            await _context.SaveChangesAsync(cancellationToken);

            return new UpdateCustomerResult(true);
        }
    }
}
