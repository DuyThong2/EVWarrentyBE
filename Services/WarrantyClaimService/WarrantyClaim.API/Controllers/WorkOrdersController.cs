using MediatR;
using Microsoft.AspNetCore.Mvc;
using WarrantyClaim.Application.CQRS.Commands.CreateWorkOrder;
using WarrantyClaim.Application.CQRS.Commands.DeleteWorkOrder;
using WarrantyClaim.Application.CQRS.Commands.UpdateWorkOrder;
using WarrantyClaim.Application.CQRS.Queries.GetWorkOrderByClaimItemId;
using WarrantyClaim.Application.Dtos;

namespace WarrantyClaim.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkOrdersController : ControllerBase
    {
        private readonly ISender _sender;

        public WorkOrdersController(ISender sender)
        {
            _sender = sender;
        }

        // GET: /api/workorders/by-claimitem/{claimItemId}
        [HttpGet("by-claimitem/{claimItemId:guid}")]
        public async Task<IActionResult> GetByClaimItemId(Guid claimItemId, CancellationToken cancellationToken = default)
        {
            if (claimItemId == Guid.Empty) return BadRequest("ClaimItemId is required.");

            var result = await _sender.Send(new GetWorkOrdersByClaimItemIdQuery(claimItemId), cancellationToken);
            return Ok(result.WorkOrders);
        }

        // POST: /api/workorders
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateWorkOrderCommand command,
            CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetByClaimItemId),
                new { claimItemId = command.WorkOrder.ClaimItemId },
                new { id = result.WorkOrderId }
            );
        }

        // PUT: /api/workorders/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] WorkOrderDto dto,
            CancellationToken cancellationToken = default)
        {
            if (id != dto.Id) return BadRequest("Id in route must match Id in body.");

            var result = await _sender.Send(new UpdateWorkOrderCommand(dto), cancellationToken);
            return Ok(result); // { isUpdated = true }
        }

        // DELETE: /api/workorders/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _sender.Send(new DeleteWorkOrderCommand(id), cancellationToken);
                return Ok(result); // { isDeleted = true }
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
