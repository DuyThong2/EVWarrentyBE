using MediatR;
using Microsoft.AspNetCore.Mvc;
using WarrantyClaim.Application.CQRS.Queries.GetClaimItemById;
using WarrantyClaim.Application.CQRS.Queries.GetPartsByItemId;

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
            if (id == Guid.Empty)
            { 
                return BadRequest("Id is required."); 
            }

            
            var result = await _sender.Send(new GetClaimItemByIdQuery(id), cancellationToken);
           return Ok(result.ClaimItem);
            
            
        }

        [HttpGet("{id:guid}/parts")]
        public async Task<IActionResult> GetParts(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Item Id is required.");
            }

            
            var result = await _sender.Send(new GetPartsByItemIdQuery(id), cancellationToken);
            return Ok(result.Parts);
            
            
        }
    }
}
