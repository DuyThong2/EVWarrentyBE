using MediatR;
using Microsoft.AspNetCore.Mvc;
using PartCatalog.Application.CQRS.Commands.CreateCategory;
using PartCatalog.Application.CQRS.Commands.DeleteCategory;
using PartCatalog.Application.CQRS.Commands.UpdateCategory;
using PartCatalog.Application.CQRS.Queries.GetCategoryById;
using PartCatalog.Application.DTOs;

namespace PartCatalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ISender _sender;

        public CategoryController(ISender sender)
        {
            _sender = sender;
        }

        // ===== CREATE =====
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CategoryDto category,
            CancellationToken cancellationToken = default)
        {
            if (category is null)
                return BadRequest("Category data is required.");

            var result = await _sender.Send(new CreateCategoryCommand(category), cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = result.CateId }, result);
        }

        // ===== GET BY ID =====
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return BadRequest("Invalid Category ID.");

            try
            {
                var result = await _sender.Send(new GetCategoryByIdQuery(id), cancellationToken);
                return Ok(result.Category);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Category with id {id} not found.");
            }
        }

        // ===== UPDATE =====
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryCommand command, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return BadRequest("Invalid Category ID.");

            var result = await _sender.Send(command, cancellationToken);

            if (!result.IsSuccess)
                return NotFound($"Category with id {id} not found.");

            return Ok(result);
        }

        // ===== DELETE =====
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return BadRequest("Invalid Category ID.");

            var command = new DeleteCategoryCommand(id);
            var result = await _sender.Send(command, cancellationToken);

            if (!result.IsSuccess)
                return NotFound($"Category with id {id} not found.");

            return Ok(result);
        }
    }
}
