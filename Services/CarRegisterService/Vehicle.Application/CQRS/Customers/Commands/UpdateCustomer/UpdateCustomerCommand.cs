using Vehicle.Application.Dtos;

namespace Vehicle.Application.CQRS.Customers.Commands.UpdateCustomer
{
    public record UpdateCustomerCommand(UpdateCustomerDto Customer) : ICommand<UpdateCustomerResult>;
}
