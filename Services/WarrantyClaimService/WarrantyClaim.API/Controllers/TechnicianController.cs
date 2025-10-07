using BuildingBlocks.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WarrantyClaim.Application.CQRS.Commands.CreateTechician;
using WarrantyClaim.Application.CQRS.Commands.DeleteTechnican;
using WarrantyClaim.Application.CQRS.Commands.UpdateTechnican;
using WarrantyClaim.Application.CQRS.Queries.GetTechnicianById;
using WarrantyClaim.Application.CQRS.Queries.GetTechniciansPage;
using WarrantyClaim.Application.Dtos;

namespace WarrantyClaim.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechniciansController : ControllerBase
    {
        private readonly ISender _sender;

        public TechniciansController(ISender sender)
        {
            _sender = sender;
        }

        // GET: /api/technicians/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty) return BadRequest("Id is required.");
            try
            {
                var result = await _sender.Send(new GetTechnicianByIdQuery(id), cancellationToken);
                return Ok(result.Technician);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetByFilter(
            [FromQuery] TechniciansFilter filter,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var zeroBased = pageIndex <= 0 ? 0 : pageIndex - 1;
            var pagination = new PaginationRequest(zeroBased, pageSize);

            // sort mặc định (tuỳ enum SortOption bạn khai báo)
            var sort = new SortOption(TechSortBy.FullName, TechSortDir.Asc);

            var result = await _sender.Send(
                new GetTechniciansFilteredQuery(filter, pagination, sort),
                cancellationToken
            );

            return Ok(result.Result); // PaginatedResult<TechnicianDto>
        }

        // POST: /api/technicians
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] TechnicianDto technician,
            CancellationToken cancellationToken = default)
        {

            var result = await _sender.Send(new CreateTechnicianCommand(technician), cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.TechnicianId },
                new { id = result.TechnicianId }
            );
        }

        // PUT: /api/technicians/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] TechnicianDto technician,
            CancellationToken cancellationToken = default)
        {

            technician.Id = id;
            var result = await _sender.Send(new UpdateTechnicianCommand(technician), cancellationToken);
            return Ok(result); // { isUpdated = true }
        }

        // DELETE: /api/technicians/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty) return BadRequest("Id is required.");
            try
            {
                var result = await _sender.Send(new DeleteTechnicianCommand(id), cancellationToken);
                return Ok(result); // { isDeleted = true }
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
