using MediatR;
using Microsoft.AspNetCore.Mvc;
using WarrantyClaim.Application.Dtos;
using WarrantyClaim.Application.CQRS.Commands.CreateClaimItem;
using WarrantyClaim.Application.CQRS.Commands.UpdateClaimItem;
using WarrantyClaim.Application.CQRS.Commands.DeleteClaimItem;
using WarrantyClaim.Application.CQRS.Queries.GetClaimItemById;

namespace WarrantyClaim.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimItemsController : ControllerBase
    {
        private readonly ISender _sender;

        public ClaimItemsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty) return BadRequest("Id is required.");

            try
            {
                var result = await _sender.Send(new GetClaimItemByIdQuery(id), cancellationToken);
                return Ok(result.ClaimItem);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateClaimItemCommand command,
            CancellationToken cancellationToken = default)
        {

            var result = await _sender.Send(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.ClaimItemId },
                new { id = result.ClaimItemId }
            );
        }

        // PUT: /api/claimitems/{id}
        // Body: ClaimItemDto (phải có Id khớp với route)
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateClaimItemDto item,
            CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new UpdateClaimItemCommand(item), cancellationToken);
            return Ok(result); // { isUpdated = true }
        }

        // DELETE: /api/claimitems/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {

            try
            {
                var result = await _sender.Send(new DeleteClaimItemCommand(id), cancellationToken);
                return Ok(result); // { isDeleted = true }
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
