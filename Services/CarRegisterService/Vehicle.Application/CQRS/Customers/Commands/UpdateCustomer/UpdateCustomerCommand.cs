using Vehicle.Application.Dtos;

namespace Vehicle.Application.CQRS.Customers.Commands.UpdateCustomer
{
    public record UpdateCustomerCommand(CustomerDto Customer) : ICommand<UpdateCustomerResult>;
}
