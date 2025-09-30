using System;

namespace Vehicle.Application.CQRS.Customers.Queries.GetCustomerById
{
    public record GetCustomerByIdQuery(Guid CustomerId) : IQuery<GetCustomerByIdResult>;
}
