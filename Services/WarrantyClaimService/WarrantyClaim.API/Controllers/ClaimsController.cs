using BuildingBlocks.Pagination;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarrantyClaim.Application.CQRS.Queries.GetClaimByFilter;
using WarrantyClaim.Application.CQRS.Queries.GetClaimsByPeriod;
using WarrantyClaim.Application.CQRS.Queries.GetClaimsPage;
using WarrantyClaim.Application.WarrantyClaim.Queries.GetClaimById;

namespace WarrantyClaim.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimsController : ControllerBase
    {
        private readonly ISender _sender;

        public ClaimsController(ISender sender)
        {
            _sender = sender;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            // API nhận 1-based -> PaginationRequest dùng 0-based
            var zeroBased = pageIndex <= 0 ? 0 : pageIndex - 1;
            var pagination = new PaginationRequest(zeroBased, pageSize);

            var result = await _sender.Send(new GetClaimsPageQuery(pagination), cancellationToken);
            return Ok(result.Result); // PaginatedResult<Claim>
        }

        // GET: /api/claims/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return BadRequest("Id is required.");

            try
            {
                var result = await _sender.Send(new GetClaimByIdQuery(id), cancellationToken);
                return Ok(result.Claim);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // GET: /api/claims/by-period?start=2025-01-01&end=2025-12-31&pageIndex=1&pageSize=10
        [HttpGet("by-period")]
        public async Task<IActionResult> GetByPeriod(
            [FromQuery] DateTime start,
            [FromQuery] DateTime end,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            if (end < start)
                return BadRequest("`end` must be greater than or equal to `start`.");

            var zeroBased = pageIndex <= 0 ? 0 : pageIndex - 1;
            var pagination = new PaginationRequest(zeroBased, pageSize);

            var result = await _sender.Send(
                new GetClaimsByPeriodQuery(start, end, pagination),
                cancellationToken
            );

            return Ok(result.Result); 
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetByFilter(
            [FromQuery] ClaimsFilter filter,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var zeroBased = pageIndex <= 0 ? 0 : pageIndex - 1;
            var pagination = new PaginationRequest(zeroBased, pageSize);

            var sort = new SortOption(SortBy.LastModified, SortDir.Desc);
            var include = new IncludeOption(
                Technician: true,
                Items: false,
                WorkOrdersWithTech: false
            );

            var result = await _sender.Send(
                new GetClaimsFilteredQuery(filter, pagination, sort, include),
                cancellationToken
            );

            return Ok(result.Result);
        }
    }
}


