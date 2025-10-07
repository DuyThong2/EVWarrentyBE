using AutoMapper;
using BuildingBlocks.Pagination;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vehicle.Application.Dtos;
using Vehicle.Application.Filters;
using Vehicle.Application.Repositories;
using Vehicle.Domain.Enums;

namespace Vehicle.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleImagesController : ControllerBase
    {
        private readonly IVehicleImageRepository _repository;
        private readonly IMapper _mapper;

        public VehicleImagesController(IVehicleImageRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var page = await _repository.GetPagedAsync(pageIndex, pageSize, cancellationToken);
            var data = page.Data.Select(e => _mapper.Map<VehicleImageDto>(e));
            var result = new PaginatedResult<VehicleImageDto>(page.PageIndex, page.PageSize, page.Count, data);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return BadRequest("Id is required.");

            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            if (entity is null)
                return NotFound();

            return Ok(_mapper.Map<VehicleImageDto>(entity));
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetByFilter(
            [FromQuery] VehicleImageFilter filter,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var page = await _repository.FilterAsync(filter, pageIndex, pageSize, cancellationToken);
            var data = page.Data.Select(e => _mapper.Map<VehicleImageDto>(e));
            var result = new PaginatedResult<VehicleImageDto>(page.PageIndex, page.PageSize, page.Count, data);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateVehicleImageDto payload,
            CancellationToken cancellationToken = default)
        {
            if (payload is null)
                return BadRequest("Body is required.");

            var entity = _mapper.Map<Vehicle.Domain.Models.VehicleImage>(payload);
            entity.ImageId = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            var id = await _repository.CreateAsync(entity, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = id },
                new { id = id }
            );
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] VehicleImageDto payload,
            CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty || payload is null)
                return BadRequest();

            var existing = await _repository.GetByIdAsync(id, cancellationToken);
            if (existing is null)
                return NotFound();

            var entity = _mapper.Map<Vehicle.Domain.Models.VehicleImage>(payload);
            entity.ImageId = id;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.CreatedAt = existing.CreatedAt;

            await _repository.UpdateAsync(entity, cancellationToken);
            return Ok(new { isUpdated = true });
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                return BadRequest();

            var ok = await _repository.DeleteAsync(id, cancellationToken);
            if (!ok)
                return NotFound();
            return Ok(new { isDeleted = true });
        }

        // PATCH: /api/vehicleimages/{id}/toggle-delete
        [HttpPatch("{id:guid}/toggle-delete")]
        public async Task<IActionResult> ToggleDelete(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                return BadRequest();

            var existing = await _repository.GetByIdAsync(id, cancellationToken);
            if (existing is null)
                return NotFound();

            var isDeleted = existing.Status == VehicleImageStatus.Deleted;
            existing.Status = isDeleted ? VehicleImageStatus.Active : VehicleImageStatus.Deleted;
            existing.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(existing, cancellationToken);
            return Ok(new { status = existing.Status.ToString() });
        }
    }
}


