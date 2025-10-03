using MassTransit.Mediator;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PartCatalog.Application.Commands.CreatePackage;
using PartCatalog.Application.CQRS.Commands.DeletePackage;
using PartCatalog.Application.DTOs;
using PartCatalog.Applications.UpdatePackage;
using PartCatalog.Domain.Models;

namespace PartCatalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly ISender _sender;
        public PackageController(ISender sender)
        {
            _sender = sender;
        }

        // ===== Create =====
        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreatePackageDto package, CancellationToken cancellationToken = default)
        {
            if (package is null)
                return BadRequest("Part data is required.");
            var result = await _sender.Send(new CreatePackageCommand(package));

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(new { result.PackageId, result.Message });
        }

        // ===== Update =====
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdatePackageDto package)
        {
            if (id == Guid.Empty)
                return BadRequest();

            var result = await _sender.Send(new UpdatePackageCommand(package));

            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Message);
        }

        // ===== Delete =====
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                return BadRequest("Invalid id.");

            var command = new DeletePackageCommand(id);
            var result = await _sender.Send(command, cancellationToken);

            if (!result.IsSuccess)
                return NotFound($"Package with id {id} not found.");

            return Ok(result);
        }
    }
}
