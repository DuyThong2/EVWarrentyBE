using AutoMapper;
using BuildingBlocks.Pagination;
using BuildingBlocks.Exceptions;
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
            try
            {
                if (pageIndex < 1)
                    throw new BadRequestException("Page index must be greater than 0");
                if (pageSize < 1 || pageSize > 100)
                    throw new BadRequestException("Page size must be between 1 and 100");

                var page = await _repository.GetPagedAsync(pageIndex, pageSize, cancellationToken);
                var data = page.Data.Select(e => _mapper.Map<VehicleImageDto>(e));
                var result = new PaginatedResult<VehicleImageDto>(page.PageIndex, page.PageSize, page.Count, data);
                return Ok(result);
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InternalServerException("An error occurred while retrieving vehicle images", ex.Message);
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new BadRequestException("Vehicle image ID is required");

                var entity = await _repository.GetByIdAsync(id, cancellationToken);
                if (entity is null)
                    throw new NotFoundException("Vehicle image", id);

                return Ok(_mapper.Map<VehicleImageDto>(entity));
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
                throw new InternalServerException("An error occurred while retrieving the vehicle image", ex.Message);
            }
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetByFilter(
            [FromQuery] VehicleImageFilter filter,
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
                var data = page.Data.Select(e => _mapper.Map<VehicleImageDto>(e));
                var result = new PaginatedResult<VehicleImageDto>(page.PageIndex, page.PageSize, page.Count, data);
                return Ok(result);
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InternalServerException("An error occurred while filtering vehicle images", ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateVehicleImageDto payload,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (payload is null)
                    throw new BadRequestException("Request body is required");

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
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InternalServerException("An error occurred while creating the vehicle image", ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] VehicleImageDto payload,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new BadRequestException("Vehicle image ID is required");
                if (payload is null)
                    throw new BadRequestException("Request body is required");

                var existing = await _repository.GetByIdAsync(id, cancellationToken);
                if (existing is null)
                    throw new NotFoundException("Vehicle image", id);

                var entity = _mapper.Map<Vehicle.Domain.Models.VehicleImage>(payload);
                entity.ImageId = id;
                entity.UpdatedAt = DateTime.UtcNow;
                entity.CreatedAt = existing.CreatedAt;

                await _repository.UpdateAsync(entity, cancellationToken);
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
                throw new InternalServerException("An error occurred while updating the vehicle image", ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new BadRequestException("Vehicle image ID is required");

                var ok = await _repository.DeleteAsync(id, cancellationToken);
                if (!ok)
                    throw new NotFoundException("Vehicle image", id);

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
                throw new InternalServerException("An error occurred while deleting the vehicle image", ex.Message);
            }
        }

        // PATCH: /api/vehicleimages/{id}/toggle-delete
        [HttpPatch("{id:guid}/toggle-delete")]
        public async Task<IActionResult> ToggleDelete(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new BadRequestException("Vehicle image ID is required");

                var existing = await _repository.GetByIdAsync(id, cancellationToken);
                if (existing is null)
                    throw new NotFoundException("Vehicle image", id);

                var isDeleted = existing.Status == VehicleImageStatus.Deleted;
                existing.Status = isDeleted ? VehicleImageStatus.Active : VehicleImageStatus.Deleted;
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
                throw new InternalServerException("An error occurred while toggling vehicle image delete status", ex.Message);
            }
        }
    }
}


