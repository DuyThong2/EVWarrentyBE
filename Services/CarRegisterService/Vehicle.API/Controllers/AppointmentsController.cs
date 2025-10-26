using AutoMapper;
using BuildingBlocks.Exceptions;
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
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentRepository _repository;
        private readonly IMapper _mapper;

        public AppointmentsController(IAppointmentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: /api/appointments
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
                var data = page.Data.Select(e => _mapper.Map<AppointmentDto>(e));
                var result = new PaginatedResult<AppointmentDto>(page.PageIndex, page.PageSize, page.Count, data);
                return Ok(result);
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InternalServerException("An error occurred while retrieving appointments", ex.Message);
            }
        }

        // GET: /api/appointments/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new BadRequestException("Appointment ID is required");

                var entity = await _repository.GetByIdAsync(id, cancellationToken);

                if (entity is null)
                    throw new NotFoundException("Appointment", id);

                return Ok(_mapper.Map<AppointmentDto>(entity));
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
                throw new InternalServerException("An error occurred while retrieving the appointment", ex.Message);
            }
        }

        // GET: /api/appointments/by-vehicle/{vehicleId}
        [HttpGet("by-vehicle/{vehicleId:guid}")]
        public async Task<IActionResult> GetByVehicleId(
            Guid vehicleId,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (vehicleId == Guid.Empty)
                    throw new BadRequestException("Vehicle ID is required");
                if (pageIndex < 1)
                    throw new BadRequestException("Page index must be greater than 0");
                if (pageSize < 1 || pageSize > 100)
                    throw new BadRequestException("Page size must be between 1 and 100");

                var page = await _repository.GetByVehicleIdAsync(vehicleId, pageIndex, pageSize, cancellationToken);
                var data = page.Data.Select(e => _mapper.Map<AppointmentDto>(e));
                var result = new PaginatedResult<AppointmentDto>(page.PageIndex, page.PageSize, page.Count, data);
                return Ok(result);
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InternalServerException("An error occurred while retrieving appointments by vehicle", ex.Message);
            }
        }

        // GET: /api/appointments/by-date-range
        [HttpGet("by-date-range")]
        public async Task<IActionResult> GetByDateRange(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
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
                if (startDate >= endDate)
                    throw new BadRequestException("Start date must be less than end date");

                var page = await _repository.GetByDateRangeAsync(startDate, endDate, pageIndex, pageSize, cancellationToken);
                var data = page.Data.Select(e => _mapper.Map<AppointmentDto>(e));
                var result = new PaginatedResult<AppointmentDto>(page.PageIndex, page.PageSize, page.Count, data);
                return Ok(result);
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InternalServerException("An error occurred while retrieving appointments by date range", ex.Message);
            }
        }

        // GET: /api/appointments/filter
        [HttpGet("filter")]
        public async Task<IActionResult> GetByFilter(
            [FromQuery] AppointmentFilter filter,
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
                var data = page.Data.Select(e => _mapper.Map<AppointmentDto>(e));
                var result = new PaginatedResult<AppointmentDto>(page.PageIndex, page.PageSize, page.Count, data);
                return Ok(result);
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InternalServerException("An error occurred while filtering appointments", ex.Message);
            }
        }

        // POST: /api/appointments
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateAppointmentDto payload,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (payload is null)
                    throw new BadRequestException("Request body is required");

                // Validate that ScheduledDateTime is not in the past
                if (payload.ScheduledDateTime <= DateTime.UtcNow)
                    throw new BadRequestException("Scheduled date and time must be in the future");

                var entity = _mapper.Map<Vehicle.Domain.Models.Appointment>(payload);
                entity.AppointmentId = Guid.NewGuid();
                entity.Status = AppointmentStatus.Scheduled;
                entity.CreatedAt = DateTime.UtcNow;
                entity.UpdatedAt = DateTime.UtcNow;

                await _repository.AddAsync(entity, cancellationToken);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = entity.AppointmentId },
                    new { id = entity.AppointmentId }
                );
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InternalServerException("An error occurred while creating the appointment", ex.Message);
            }
        }

        // PUT: /api/appointments/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateAppointmentDto payload,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new BadRequestException("Appointment ID is required");
                if (payload is null)
                    throw new BadRequestException("Request body is required");

                var existing = await _repository.GetByIdAsync(id, cancellationToken);
                if (existing is null)
                    throw new NotFoundException("Appointment", id);

                var createdAt = existing.CreatedAt;
                _mapper.Map(payload, existing);
                existing.AppointmentId = id;
                existing.UpdatedAt = DateTime.UtcNow;
                existing.CreatedAt = createdAt;

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
                throw new InternalServerException("An error occurred while updating the appointment", ex.Message);
            }
        }

        // DELETE: /api/appointments/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new BadRequestException("Appointment ID is required");

                var ok = await _repository.DeleteAsync(id, cancellationToken);
                if (!ok)
                    throw new NotFoundException("Appointment", id);

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
                throw new InternalServerException("An error occurred while deleting the appointment", ex.Message);
            }
        }

        // PATCH: /api/appointments/{id}/toggle-delete
        [HttpPatch("{id:guid}/toggle-delete")]
        public async Task<IActionResult> ToggleDelete(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new BadRequestException("Appointment ID is required");

                var existing = await _repository.GetByIdAsync(id, cancellationToken);
                if (existing is null)
                    throw new NotFoundException("Appointment", id);

                var isDeleted = existing.Status == AppointmentStatus.Deleted;
                existing.Status = isDeleted ? AppointmentStatus.Scheduled : AppointmentStatus.Deleted;
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
                throw new InternalServerException("An error occurred while toggling appointment delete status", ex.Message);
            }
        }
    }
}
