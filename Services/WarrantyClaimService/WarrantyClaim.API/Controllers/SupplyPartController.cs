using MediatR;
using Microsoft.AspNetCore.Mvc;
using WarrantyClaim.Application.Dtos;
using WarrantyClaim.Application.CQRS.Commands.CreateSupplyPart;
using WarrantyClaim.Application.CQRS.Commands.UpdateSupplyPart;
using WarrantyClaim.Application.CQRS.Commands.DeleteSupplyPart;
using WarrantyClaim.Application.CQRS.Queries.GetPartsByItemId;

namespace WarrantyClaim.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplyPartsController : ControllerBase
    {
        private readonly ISender _sender;

        public SupplyPartsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("by-item/{claimItemId:guid}")]
        public async Task<IActionResult> GetByItemId(Guid claimItemId, CancellationToken cancellationToken = default)
        {

            var result = await _sender.Send(new GetPartsByItemIdQuery(claimItemId), cancellationToken);
            return Ok(result.Parts);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateSupplyPartCommand command,
            CancellationToken cancellationToken = default)
        {
            if (command is null) return BadRequest("Body is required.");

            var result = await _sender.Send(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetByItemId),
                new { claimItemId = command.SupplyPart.ClaimItemId },
                new { id = result.PartSupplyId }
            );
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] PartSupplyDto dto,
            CancellationToken cancellationToken = default)
        {


            var result = await _sender.Send(new UpdateSupplyPartCommand(dto), cancellationToken);
            return Ok(result); 
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty) return BadRequest("Id is required.");

            try
            {
                var result = await _sender.Send(new DeleteSupplyPartCommand(id), cancellationToken);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
