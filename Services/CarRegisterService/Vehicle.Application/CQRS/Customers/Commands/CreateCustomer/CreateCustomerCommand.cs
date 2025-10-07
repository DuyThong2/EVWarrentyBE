using System;
using Vehicle.Application.Dtos;

namespace Vehicle.Application.CQRS.Customers.Commands.CreateCustomer
{
    public record CreateCustomerCommand(CreateCustomerDto Customer) : ICommand<CreateCustomerResult>;
}
