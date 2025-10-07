using AutoMapper;
using AutoMapper.QueryableExtensions;
using BuildingBlocks.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vehicle.Application.Dtos;
using Vehicle.Domain.Enums;
using Vehicle.Domain.Enums;
using Vehicle.Application.Filters;
using Vehicle.Application.Repositories;

namespace Vehicle.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleRepository _repository;
        private readonly IMapper _mapper;

        public VehiclesController(IVehicleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // Filter type moved to Application.Filters.VehicleFilter

        // GET: /api/vehicles
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var page = await _repository.GetPagedAsync(pageIndex, pageSize, cancellationToken);
            var data = page.Data.Select(e => _mapper.Map<VehicleDto>(e));
            var result = new PaginatedResult<VehicleDto>(page.PageIndex, page.PageSize, page.Count, data);
            return Ok(result);
        }

        // GET: /api/vehicles/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return BadRequest("Id is required.");

            var entity = await _repository.GetByIdAsync(id, cancellationToken);

            if (entity is null)
                return NotFound();

            return Ok(_mapper.Map<VehicleDto>(entity));
        }

        // GET: /api/vehicles/filter
        [HttpGet("filter")]
        public async Task<IActionResult> GetByFilter(
            [FromQuery] VehicleFilter filter,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var page = await _repository.FilterAsync(filter, pageIndex, pageSize, cancellationToken);
            var data = page.Data.Select(e => _mapper.Map<VehicleDto>(e));
            var result = new PaginatedResult<VehicleDto>(page.PageIndex, page.PageSize, page.Count, data);
            return Ok(result);
        }

        // GET: /api/vehicles/by-customer/{customerId}
        [HttpGet("by-customer/{customerId:guid}")]
        public async Task<IActionResult> GetByCustomerId(
            Guid customerId,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            if (customerId == Guid.Empty)
                return BadRequest("customerId is required.");

            var filter = new VehicleFilter { CustomerId = customerId };
            var page = await _repository.FilterAsync(filter, pageIndex, pageSize, cancellationToken);
            var data = page.Data.Select(e => _mapper.Map<VehicleDto>(e));
            var result = new PaginatedResult<VehicleDto>(page.PageIndex, page.PageSize, page.Count, data);
            return Ok(result);
        }

        // POST: /api/vehicles
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateVehicleDto payload,
            CancellationToken cancellationToken = default)
        {
            if (payload is null)
                return BadRequest("Body is required.");

            var entity = _mapper.Map<Vehicle.Domain.Models.Vehicle>(payload);
            entity.VehicleId = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            var id = await _repository.CreateAsync(entity, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = id },
                new { id = id }
            );
        }

        // PUT: /api/vehicles/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateVehicleDto payload,
            CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty || payload is null)
                return BadRequest();

            var existing = await _repository.GetByIdAsync(id, cancellationToken);
            if (existing is null)
                return NotFound();
            var createdAt = existing.CreatedAt; // lưu lại trước khi map
            _mapper.Map(payload, existing);
            existing.VehicleId = id;
            existing.UpdatedAt = DateTime.UtcNow;
            existing.CreatedAt = createdAt; // khôi phục lại giá trị gốc

            await _repository.UpdateAsync(existing, cancellationToken);
            return Ok(new { isUpdated = true });
        }

        // DELETE: /api/vehicles/{id}
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

        // PATCH: /api/vehicles/{id}/toggle-delete
        [HttpPatch("{id:guid}/toggle-delete")]
        public async Task<IActionResult> ToggleDelete(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                return BadRequest();

            var existing = await _repository.GetByIdAsync(id, cancellationToken);
            if (existing is null)
                return NotFound();

            var isDeleted = existing.Status == VehicleStatus.Deleted;
            existing.Status = isDeleted ? VehicleStatus.Active : VehicleStatus.Deleted;
            existing.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(existing, cancellationToken);
            return Ok(new { status = existing.Status.ToString() });
        }
    }
}


