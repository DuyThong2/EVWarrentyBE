using AutoMapper;

using Vehicle.Application.CQRS.Customers.Commands.CreateCustomer;
using Vehicle.Application.Data;
using Vehicle.Domain.Models;

namespace Vehicle.Application.Commands
{
    public class CreateCustomerHandler : ICommandHandler<CreateCustomerCommand, CreateCustomerResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateCustomerHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CreateCustomerResult> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customerId = Guid.NewGuid();

            var customer = _mapper.Map<Customer>(request.Customer);
            customer.CustomerId = customerId;
            customer.CreatedAt = DateTime.UtcNow;
            customer.UpdatedAt = DateTime.UtcNow;

            foreach (var vehicle in customer.Vehicles)
            {
                vehicle.VehicleId = Guid.NewGuid();
                vehicle.CustomerId = customerId;
                vehicle.CreatedAt = DateTime.UtcNow;
                vehicle.UpdatedAt = DateTime.UtcNow;

                foreach (var part in vehicle.Parts)
                {
                    part.PartId = Guid.NewGuid();
                    part.VehicleId = vehicle.VehicleId;
                   
                }
            }

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync(cancellationToken);

            return new CreateCustomerResult(customer.CustomerId);
        }
    }
}
