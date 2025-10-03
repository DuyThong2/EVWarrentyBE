using System;

namespace Vehicle.Application.CQRS.Customers.Commands.CreateCustomer
{
    public record CreateCustomerResult(Guid CustomerId);
}
