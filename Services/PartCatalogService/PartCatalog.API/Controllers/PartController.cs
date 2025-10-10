using AutoMapper;
using BuildingBlocks.Pagination;
using MassTransit.Mediator;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PartCatalog.Application.CQRS.Commands.CreatePart;
using PartCatalog.Application.CQRS.Commands.DeletePart;
using PartCatalog.Application.CQRS.Commands.UpdatePart;
using PartCatalog.Application.CQRS.Queries.GetAllParts;
using PartCatalog.Application.CQRS.Queries.GetPartByFilter;
using PartCatalog.Application.CQRS.Queries.GetPartById;
using PartCatalog.Application.DTOs;

namespace PartCatalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartController : ControllerBase
    {
        private readonly ISender _sender;

        public PartController(ISender sender)
        {
            _sender = sender;
        }

        // ===== Create =====
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreatePartDto part,
            CancellationToken cancellationToken = default)
        {
            if (part is null)
                return BadRequest("Part data is required.");

            var result = await _sender.Send(new CreatePartCommand(part), cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.PartId },
                new { id = result.PartId }
            );
        }

        // API Part/GetById/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return BadRequest("Id is required.");

            try
            {
                var result = await _sender.Send(new GetPartByIdQuery(id), cancellationToken);
                return Ok(result.Part);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // ===== Update =====
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdatePart(Guid id, [FromBody] UpdatePartCommand command)
        {
            if (id == Guid.Empty)
                return BadRequest();

            var result = await _sender.Send(command);

            if (!result.IsSuccess)
                return NotFound($"Part with id {id} not found.");
            return Ok(result);

        }

        // ===== DELETE =====
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletePart(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest();

            var command = new DeletePartCommand(id);
            var result = await _sender.Send(command);

            if (!result.IsSuccess)
                return NotFound($"Part with id {id} not found.");

            return Ok(result);
        }

        // Part/GetByFilter 
        [HttpGet("filter")]
        public async Task<IActionResult> GetByFilter(
            [FromQuery] string? Name,
            [FromQuery] string? CateName,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(
                new GetPartByFilterQuery(Name, CateName, pageIndex, pageSize),
                cancellationToken
            );
            return Ok(result);
        }


        // API Part/GetAll
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetAllPartsQuery(pageIndex, pageSize), cancellationToken);
            return Ok(result);
        }
    }
}
