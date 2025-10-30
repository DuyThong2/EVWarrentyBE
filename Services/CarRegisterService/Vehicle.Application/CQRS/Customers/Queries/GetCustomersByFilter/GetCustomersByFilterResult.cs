using BuildingBlocks.Pagination;
using Vehicle.Application.Dtos;

namespace Vehicle.Application.CQRS.Customers.Queries.GetCustomersByFilter
{
    public record GetCustomersByFilterResult(PaginatedResult<CustomerDto> Result);
}
