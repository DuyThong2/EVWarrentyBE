using AutoMapper;
using AutoMapper.QueryableExtensions;
using BuildingBlocks.Pagination;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vehicle.Application.Dtos;
using Vehicle.Domain.Enums;
using Vehicle.Application.Filters;
using Vehicle.Application.Repositories;

namespace Vehicle.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclePartsController : ControllerBase
    {
        private readonly IVehiclePartRepository _repository;
        private readonly IMapper _mapper;

        public VehiclePartsController(IVehiclePartRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        // Filter type moved to Application.Filters.VehiclePartFilter

        // GET: /api/vehicleparts
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var page = await _repository.GetPagedAsync(pageIndex, pageSize, cancellationToken);
            var data = page.Data.Select(e => _mapper.Map<VehiclePartDto>(e));
            var result = new PaginatedResult<VehiclePartDto>(page.PageIndex, page.PageSize, page.Count, data);
            return Ok(result);
        }

        // GET: /api/vehicleparts/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return BadRequest("Id is required.");

            var entity = await _repository.GetByIdAsync(id, cancellationToken);

            if (entity is null)
                return NotFound();

            return Ok(_mapper.Map<VehiclePartDto>(entity));
        }

        // GET: /api/vehicleparts/filter
        [HttpGet("filter")]
        public async Task<IActionResult> GetByFilter(
            [FromQuery] VehiclePartFilter filter,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var page = await _repository.FilterAsync(filter, pageIndex, pageSize, cancellationToken);
            var data = page.Data.Select(e => _mapper.Map<VehiclePartDto>(e));
            var result = new PaginatedResult<VehiclePartDto>(page.PageIndex, page.PageSize, page.Count, data);
            return Ok(result);
        }

        // POST: /api/vehicleparts
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateVehiclePartDto payload,
            CancellationToken cancellationToken = default)
        {
            if (payload is null)
                return BadRequest("Body is required.");

            var entity = _mapper.Map<Vehicle.Domain.Models.VehiclePart>(payload);
            entity.PartId = Guid.NewGuid();

            var id = await _repository.CreateAsync(entity, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = id },
                new { id = id }
            );
        }

        // PUT: /api/vehicleparts/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] VehiclePartDto payload,
            CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty || payload is null)
                return BadRequest();

            var existing = await _repository.GetByIdAsync(id, cancellationToken);
            if (existing is null)
                return NotFound();

            _mapper.Map(payload, existing);
            existing.PartId = id;

            await _repository.UpdateAsync(existing, cancellationToken);
            return Ok(new { isUpdated = true });
        }

        // DELETE: /api/vehicleparts/{id}
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
    }
}


