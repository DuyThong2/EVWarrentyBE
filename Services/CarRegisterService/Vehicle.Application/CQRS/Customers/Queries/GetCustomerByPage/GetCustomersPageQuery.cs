using BuildingBlocks.Pagination;
using MediatR;
using Vehicle.Application.Dtos;

namespace Vehicle.Application.CQRS.Customers.Queries.GetCustomersPage
{
    public record GetCustomersPageQuery(PaginationRequest PaginationRequest) : IRequest<GetCustomersPageResult>;

    public record GetCustomersPageResult(PaginatedResult<CustomerDto> Result);
}
