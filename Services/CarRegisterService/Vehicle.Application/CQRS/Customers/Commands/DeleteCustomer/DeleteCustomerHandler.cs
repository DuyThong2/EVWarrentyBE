using Microsoft.EntityFrameworkCore;

using Vehicle.Application.CQRS.Customers.Commands.DeleteCustomer;
using Vehicle.Application.Data;

namespace Vehicle.Application.Commands
{
    public class DeleteCustomerHandler : ICommandHandler<DeleteCustomerCommand, DeleteCustomerResult>
    {
        private readonly IApplicationDbContext _context;

        public DeleteCustomerHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DeleteCustomerResult> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers
                .Include(c => c.Vehicles)
                    .ThenInclude(v => v.Parts)
                .FirstOrDefaultAsync(c => c.CustomerId == request.CustomerId, cancellationToken);

            if (customer == null)
                throw new KeyNotFoundException($"Customer {request.CustomerId} not found.");

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync(cancellationToken);

            return new DeleteCustomerResult(true);
        }
    }
}
