using System;

namespace Vehicle.Application.CQRS.Customers.Commands.DeleteCustomer
{
    public record DeleteCustomerCommand(Guid CustomerId) : ICommand<DeleteCustomerResult>;
}
