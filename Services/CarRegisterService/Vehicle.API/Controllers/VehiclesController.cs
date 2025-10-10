using AutoMapper;
using AutoMapper.QueryableExtensions;
using BuildingBlocks.Pagination;
using BuildingBlocks.Exceptions;
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
            try
            {
                if (pageIndex < 1)
                    throw new BadRequestException("Page index must be greater than 0");
                if (pageSize < 1 || pageSize > 100)
                    throw new BadRequestException("Page size must be between 1 and 100");

                var page = await _repository.GetPagedAsync(pageIndex, pageSize, cancellationToken);
                var data = page.Data.Select(e => _mapper.Map<VehicleDto>(e));
                var result = new PaginatedResult<VehicleDto>(page.PageIndex, page.PageSize, page.Count, data);
                return Ok(result);
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InternalServerException("An error occurred while retrieving vehicles", ex.Message);
            }
        }

        // GET: /api/vehicles/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new BadRequestException("Vehicle ID is required");

                var entity = await _repository.GetByIdAsync(id, cancellationToken);

                if (entity is null)
                    throw new NotFoundException("Vehicle", id);

                return Ok(_mapper.Map<VehicleDto>(entity));
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
                throw new InternalServerException("An error occurred while retrieving the vehicle", ex.Message);
            }
        }

        // GET: /api/vehicles/filter
        [HttpGet("filter")]
        public async Task<IActionResult> GetByFilter(
            [FromQuery] VehicleFilter filter,
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
                var data = page.Data.Select(e => _mapper.Map<VehicleDto>(e));
                var result = new PaginatedResult<VehicleDto>(page.PageIndex, page.PageSize, page.Count, data);
                return Ok(result);
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InternalServerException("An error occurred while filtering vehicles", ex.Message);
            }
        }

        // GET: /api/vehicles/by-customer/{customerId}
        [HttpGet("by-customer/{customerId:guid}")]
        public async Task<IActionResult> GetByCustomerId(
            Guid customerId,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (customerId == Guid.Empty)
                    throw new BadRequestException("Customer ID is required");
                if (pageIndex < 1)
                    throw new BadRequestException("Page index must be greater than 0");
                if (pageSize < 1 || pageSize > 100)
                    throw new BadRequestException("Page size must be between 1 and 100");

                var filter = new VehicleFilter { CustomerId = customerId };
                var page = await _repository.FilterAsync(filter, pageIndex, pageSize, cancellationToken);
                var data = page.Data.Select(e => _mapper.Map<VehicleDto>(e));
                var result = new PaginatedResult<VehicleDto>(page.PageIndex, page.PageSize, page.Count, data);
                return Ok(result);
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InternalServerException("An error occurred while retrieving vehicles by customer", ex.Message);
            }
        }

        // POST: /api/vehicles
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateVehicleDto payload,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (payload is null)
                    throw new BadRequestException("Request body is required");

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
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InternalServerException("An error occurred while creating the vehicle", ex.Message);
            }
        }

        // PUT: /api/vehicles/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateVehicleDto payload,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new BadRequestException("Vehicle ID is required");
                if (payload is null)
                    throw new BadRequestException("Request body is required");

                var existing = await _repository.GetByIdAsync(id, cancellationToken);
                if (existing is null)
                    throw new NotFoundException("Vehicle", id);

                var createdAt = existing.CreatedAt; // lưu lại trước khi map
                _mapper.Map(payload, existing);
                existing.VehicleId = id;
                existing.UpdatedAt = DateTime.UtcNow;
                existing.CreatedAt = createdAt; // khôi phục lại giá trị gốc

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
                throw new InternalServerException("An error occurred while updating the vehicle", ex.Message);
            }
        }

        // DELETE: /api/vehicles/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new BadRequestException("Vehicle ID is required");

                var ok = await _repository.DeleteAsync(id, cancellationToken);
                if (!ok)
                    throw new NotFoundException("Vehicle", id);

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
                throw new InternalServerException("An error occurred while deleting the vehicle", ex.Message);
            }
        }

        // PATCH: /api/vehicles/{id}/toggle-delete
        [HttpPatch("{id:guid}/toggle-delete")]
        public async Task<IActionResult> ToggleDelete(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new BadRequestException("Vehicle ID is required");

                var existing = await _repository.GetByIdAsync(id, cancellationToken);
                if (existing is null)
                    throw new NotFoundException("Vehicle", id);

                var isDeleted = existing.Status == VehicleStatus.Deleted;
                existing.Status = isDeleted ? VehicleStatus.Active : VehicleStatus.Deleted;
                existing.UpdatedAt = DateTime.UtcNow;

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
                throw new InternalServerException("An error occurred while toggling vehicle delete status", ex.Message);
            }
        }
    }
}


