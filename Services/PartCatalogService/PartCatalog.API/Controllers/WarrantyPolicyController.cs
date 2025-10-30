using MediatR;
using Microsoft.AspNetCore.Mvc;
using PartCatalog.Application.CQRS.Commands.CreateWarrantyPolicy;
using PartCatalog.Application.CQRS.Commands.DeleteWarrantyPolicy;
using PartCatalog.Application.CQRS.Commands.UpdateWarrantyPolicy;
using PartCatalog.Application.CQRS.Queries.GetWarrantyPolicyById;
using PartCatalog.Application.DTOs;
using PartCatalog.Application.CQRS.Queries.GetWarrantyPolicyByFilter;
using PartCatalog.Domain.Enums;

namespace PartCatalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarrantyPolicyController : ControllerBase
    {
        private readonly ISender _sender;

        public WarrantyPolicyController(ISender sender)
        {
            _sender = sender;
        }

        // ===== Create =====
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWarrantyPolicyDto policyDto, CancellationToken cancellationToken)
        {
            if (policyDto is null)
                return BadRequest("Policy data is required.");

            var result = await _sender.Send(new CreateWarrantyPolicyCommand(policyDto), cancellationToken);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return CreatedAtAction(nameof(GetById), new { id = result.PolicyId }, result);
        }

        // ===== Get By Id =====
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return BadRequest("Invalid policy id.");

            var result = await _sender.Send(new GetWarrantyPolicyByIdQuery(id), cancellationToken);

            if (result.Policy == null)
                return NotFound($"Policy with id {id} not found.");

            return Ok(result.Policy);
        }

        // ===== Get By Filter =====
        [HttpGet("filter")]
        public async Task<IActionResult> GetByFilter(
            [FromQuery] string? Code,
            [FromQuery] string? Name,
            [FromQuery] Guid? PackageId,
            [FromQuery] PolicyType? Type,
            [FromQuery] ActiveStatus? Status,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var query = new GetWarrantyPolicyByFilterQuery(Code, Name, PackageId, Type, Status, pageIndex, pageSize);
            var result = await _sender.Send(query, cancellationToken);
            return Ok(result);
        }

        // ===== Update =====
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateWarrantyPolicyCommand command, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return BadRequest("Invalid policy id.");

            if (command.Policy.PolicyId != id)
                return BadRequest("PolicyId mismatch between URL and body.");

            var result = await _sender.Send(command, cancellationToken);

            if (!result.IsSuccess)
                return NotFound(result.Message);

            return Ok(result);
        }

        // ===== Delete =====
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return BadRequest("Invalid policy id.");

            var command = new DeleteWarrantyPolicyCommand(id);
            var result = await _sender.Send(command, cancellationToken);

            if (!result.IsSuccess)
                return NotFound(result.Message);

            return Ok(result);
        }
    }
}
