using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Vehicle.Domain.Enums;

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
                .FirstOrDefaultAsync(c => c.CustomerId == request.Customer.CustomerId, cancellationToken);

            if (customer == null)
                throw new KeyNotFoundException($"Customer {request.Customer.CustomerId} not found.");

            // Chỉ cập nhật các property cần thiết, không động đến collection Vehicles
            customer.FullName = request.Customer.FullName;
            customer.Email = request.Customer.Email;
            customer.PhoneNumber = request.Customer.PhoneNumber;
            customer.Address = request.Customer.Address;
            
            // Parse Status từ string sang enum
            if (Enum.TryParse<CustomerStatus>(request.Customer.Status, true, out var status))
            {
                customer.Status = status;
            }
            
            customer.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return new UpdateCustomerResult(true);
        }
    }
}
