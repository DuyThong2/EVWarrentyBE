using Vehicle.Application.Dtos;

namespace Vehicle.Application.CQRS.Customers.Queries.GetCustomerById
{
    public record GetCustomerByIdResult(CustomerDto Customer);
}
