using AutoMapper;
using AutoMapper.QueryableExtensions;
using BuildingBlocks.Pagination;
using BuildingBlocks.Exceptions;
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
            try
            {
                if (pageIndex < 1)
                    throw new BadRequestException("Page index must be greater than 0");
                if (pageSize < 1 || pageSize > 100)
                    throw new BadRequestException("Page size must be between 1 and 100");

                var page = await _repository.GetPagedAsync(pageIndex, pageSize, cancellationToken);
                var data = page.Data.Select(e => _mapper.Map<VehiclePartDto>(e));
                var result = new PaginatedResult<VehiclePartDto>(page.PageIndex, page.PageSize, page.Count, data);
                return Ok(result);
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InternalServerException("An error occurred while retrieving vehicle parts", ex.Message);
            }
        }

        // GET: /api/vehicleparts/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new BadRequestException("Vehicle part ID is required");

                var entity = await _repository.GetByIdAsync(id, cancellationToken);

                if (entity is null)
                    throw new NotFoundException("Vehicle part", id);

                return Ok(_mapper.Map<VehiclePartDto>(entity));
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InternalServerException("An error occurred while retrieving the vehicle part", ex.Message);
            }
        }

        // GET: /api/vehicleparts/filter
        [HttpGet("filter")]
        public async Task<IActionResult> GetByFilter(
            [FromQuery] VehiclePartFilter filter,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (pageIndex < 1)
                    throw new BadRequestException("Page index must be greater than 0");
                if (pageSize < 1 || pageSize > 100)
                    throw new BadRequestException("Page size must be between 1 and 100");

                var page = await _repository.FilterAsync(filter, pageIndex, pageSize, cancellationToken);
                var data = page.Data.Select(e => _mapper.Map<VehiclePartDto>(e));
                var result = new PaginatedResult<VehiclePartDto>(page.PageIndex, page.PageSize, page.Count, data);
                return Ok(result);
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InternalServerException("An error occurred while filtering vehicle parts", ex.Message);
            }
        }

        // POST: /api/vehicleparts
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateVehiclePartDto payload,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (payload is null)
                    throw new BadRequestException("Request body is required");

                var entity = _mapper.Map<Vehicle.Domain.Models.VehiclePart>(payload);
                entity.PartId = payload.PartId ?? Guid.NewGuid();

                var id = await _repository.CreateAsync(entity, cancellationToken);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = id },
                    new { id = id }
                );
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InternalServerException("An error occurred while creating the vehicle part", ex.Message);
            }
        }

        // PUT: /api/vehicleparts/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] VehiclePartDto payload,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new BadRequestException("Vehicle part ID is required");
                if (payload is null)
                    throw new BadRequestException("Request body is required");

                var existing = await _repository.GetByIdAsync(id, cancellationToken);
                if (existing is null)
                    throw new NotFoundException("Vehicle part", id);

                _mapper.Map(payload, existing);
                existing.PartId = id;

                await _repository.UpdateAsync(existing, cancellationToken);
                return Ok(new { isUpdated = true });
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InternalServerException("An error occurred while updating the vehicle part", ex.Message);
            }
        }

        // DELETE: /api/vehicleparts/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new BadRequestException("Vehicle part ID is required");

                var ok = await _repository.DeleteAsync(id, cancellationToken);
                if (!ok)
                    throw new NotFoundException("Vehicle part", id);

                return Ok(new { isDeleted = true });
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InternalServerException("An error occurred while deleting the vehicle part", ex.Message);
            }
        }

        // PATCH: /api/vehicleparts/{id}/toggle-delete
        [HttpPatch("{id:guid}/toggle-delete")]
        public async Task<IActionResult> ToggleDelete(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new BadRequestException("Vehicle part ID is required");

                var existing = await _repository.GetByIdAsync(id, cancellationToken);
                if (existing is null)
                    throw new NotFoundException("Vehicle part", id);

                var isDeleted = existing.Status == PartStatus.Scrapped || existing.Status.ToString() == "Deleted";
                existing.Status = isDeleted ? PartStatus.Installed : PartStatus.Scrapped;

                await _repository.UpdateAsync(existing, cancellationToken);
                return Ok(new { status = existing.Status.ToString() });
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InternalServerException("An error occurred while toggling vehicle part delete status", ex.Message);
            }
        }
    }
}


