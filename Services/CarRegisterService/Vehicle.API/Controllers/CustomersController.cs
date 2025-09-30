using BuildingBlocks.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using Vehicle.Application.CQRS.Customers.Commands.CreateCustomer;
using Vehicle.Application.CQRS.Customers.Commands.DeleteCustomer;
using Vehicle.Application.CQRS.Customers.Commands.UpdateCustomer;
using Vehicle.Application.CQRS.Customers.Queries.GetCustomerById;
using Vehicle.Application.CQRS.Customers.Queries.GetCustomersByFilter;
using Vehicle.Application.CQRS.Customers.Queries.GetCustomersPage;
using Vehicle.Application.Dtos;

namespace Vehicle.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ISender _sender;

        public CustomersController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
     [FromQuery] int pageIndex = 1,
     [FromQuery] int pageSize = 10,
     CancellationToken cancellationToken = default)
        {
            var zeroBased = pageIndex <= 0 ? 0 : pageIndex - 1;
            var pagination = new PaginationRequest(zeroBased, pageSize);

            var result = await _sender.Send(new GetCustomersPageQuery(pagination), cancellationToken);
            return Ok(result.Result); // trả PaginatedResult<CustomerDto>
        }

        // GET: /api/customers/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return BadRequest("Id is required.");

            try
            {
                var result = await _sender.Send(new GetCustomerByIdQuery(id), cancellationToken);
                return Ok(result.Customer);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // GET: /api/customers/filter
        [HttpGet("filter")]
        public async Task<IActionResult> GetByFilter(
            [FromQuery] CustomersFilter filter,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var zeroBased = pageIndex <= 0 ? 0 : pageIndex - 1;
            var pagination = new PaginationRequest(zeroBased, pageSize);

            var sort = new SortOption(SortBy.CreatedAt, SortDir.Desc);

            var result = await _sender.Send(
                new GetCustomersByFilterQuery(filter, pagination, sort),
                cancellationToken
            );

            return Ok(result.Result); // PaginatedResult<CustomerDto>
        }

        // POST: /api/customers
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateCustomerDto customer,
            CancellationToken cancellationToken = default)
        {
            if (customer is null)
                return BadRequest("Body is required.");

            var result = await _sender.Send(new CreateCustomerCommand(customer), cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.CustomerId },
                new { id = result.CustomerId }
            );
        }

        // PUT: /api/customers/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] CustomerDto customer,
            CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty || customer == null)
                return BadRequest();

            customer.CustomerId = id;

            var result = await _sender.Send(new UpdateCustomerCommand(customer), cancellationToken);
            return Ok(result); // { isUpdated = true }
        }

        // DELETE: /api/customers/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                return BadRequest();

            try
            {
                var result = await _sender.Send(new DeleteCustomerCommand(id), cancellationToken);
                return Ok(result); // { isDeleted = true }
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
