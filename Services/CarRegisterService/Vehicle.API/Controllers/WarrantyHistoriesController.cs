using AutoMapper;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vehicle.Application.Data;
using Vehicle.Application.Dtos;
using Vehicle.Application.Filters;
using Vehicle.Application.Repositories;
using Vehicle.Domain.Enums;

namespace Vehicle.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarrantyHistoriesController : ControllerBase
    {
        private readonly IWarrantyHistoryRepository _repository;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public WarrantyHistoriesController(IWarrantyHistoryRepository repository, IMapper mapper, IApplicationDbContext context)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
        }

        // GET: /api/warrantyhistories
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
                var data = page.Data.Select(e => _mapper.Map<WarrantyHistoryDto>(e));
                var result = new PaginatedResult<WarrantyHistoryDto>(page.PageIndex, page.PageSize, page.Count, data);
                return Ok(result);
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InternalServerException("An error occurred while retrieving warranty histories", ex.Message);
            }
        }

        // GET: /api/warrantyhistories/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new BadRequestException("WarrantyHistory ID is required");

                var entity = await _repository.GetByIdAsync(id, cancellationToken);

                if (entity is null)
                    throw new NotFoundException("WarrantyHistory", id);

                return Ok(_mapper.Map<WarrantyHistoryDto>(entity));
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
                throw new InternalServerException("An error occurred while retrieving the warranty history", ex.Message);
            }
        }

        // GET: /api/warrantyhistories/by-vehicle/{vehicleId}
        [HttpGet("by-vehicle/{vehicleId:guid}")]
        public async Task<IActionResult> GetByVehicleId(Guid vehicleId, CancellationToken cancellationToken = default)
        {
            try
            {
                if (vehicleId == Guid.Empty)
                    throw new BadRequestException("Vehicle ID is required");

                var entities = await _repository.GetByVehicleIdAsync(vehicleId, cancellationToken);
                var dtos = entities.Select(e => _mapper.Map<WarrantyHistoryDto>(e));

                return Ok(dtos);
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InternalServerException("An error occurred while retrieving warranty histories by vehicle", ex.Message);
            }
        }

        // GET: /api/warrantyhistories/by-vin/{vin}
        [HttpGet("by-vin/{vin}")]
        public async Task<IActionResult> GetByVin(string vin, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(vin))
                    throw new BadRequestException("VIN is required");

                var entities = await _repository.GetByVinAsync(vin, cancellationToken);
                var dtos = entities.Select(e => _mapper.Map<WarrantyHistoryDto>(e));

                return Ok(dtos);
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InternalServerException("An error occurred while retrieving warranty histories by VIN", ex.Message);
            }
        }

        // GET: /api/warrantyhistories/filter
        [HttpGet("filter")]
        public async Task<IActionResult> GetByFilter(
            [FromQuery] WarrantyHistoryFilter filter,
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
                var data = page.Data.Select(e => _mapper.Map<WarrantyHistoryDto>(e));
                var result = new PaginatedResult<WarrantyHistoryDto>(page.PageIndex, page.PageSize, page.Count, data);
                return Ok(result);
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InternalServerException("An error occurred while filtering warranty histories", ex.Message);
            }
        }

        // POST: /api/warrantyhistories
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateWarrantyHistoryDto payload,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (payload is null)
                    throw new BadRequestException("Request body is required");

                // Validate required fields
                if (payload.VehicleId == Guid.Empty)
                    throw new BadRequestException("VehicleId is required");

                // Check if Vehicle exists
                var vehicleExists = await _context.Vehicles.AnyAsync(v => v.VehicleId == payload.VehicleId, cancellationToken);
                if (!vehicleExists)
                    throw new BadRequestException($"Vehicle with ID {payload.VehicleId} not found");

                // Check if Part exists (if PartId is provided)
                if (payload.PartId.HasValue && payload.PartId != Guid.Empty)
                {
                    var partExists = await _context.VehicleParts.AnyAsync(p => p.PartId == payload.PartId.Value, cancellationToken);
                    if (!partExists)
                        throw new BadRequestException($"VehiclePart with ID {payload.PartId} not found");
                }

                // Parse string enums to enum types
                if (!Enum.TryParse<WarrantyEventType>(payload.EventType, true, out var eventType))
                    throw new BadRequestException($"Invalid EventType: {payload.EventType}. Valid values are: REPAIR, REPLACEMENT, INSPECTION, EXTENSION");

                if (!Enum.TryParse<WarrantyHistoryStatus>(payload.Status, true, out var status))
                    throw new BadRequestException($"Invalid Status: {payload.Status}. Valid values are: Active, Completed, Cancelled, Deleted");

                var entity = new Vehicle.Domain.Models.WarrantyHistory
                {
                    HistoryId = Guid.NewGuid(),
                    VehicleId = payload.VehicleId,
                    PartId = payload.PartId,
                    ClaimId = payload.ClaimId,
                    PolicyId = payload.PolicyId,
                    EventType = eventType,
                    Description = payload.Description,
                    PerformedBy = payload.PerformedBy,
                    ServiceCenterName = payload.ServiceCenterName,
                    WarrantyStartDate = payload.WarrantyStartDate,
                    WarrantyEndDate = payload.WarrantyEndDate,
                    Status = status,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var createdEntity = await _repository.AddAsync(entity, cancellationToken);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = createdEntity.HistoryId },
                    new { id = createdEntity.HistoryId }
                );
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InternalServerException("An error occurred while creating the warranty history", ex.Message);
            }
        }

        // PUT: /api/warrantyhistories/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateWarrantyHistoryDto payload,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new BadRequestException("WarrantyHistory ID is required");
                if (payload is null)
                    throw new BadRequestException("Request body is required");

                var existing = await _repository.GetByIdAsync(id, cancellationToken);
                if (existing is null)
                    throw new NotFoundException("WarrantyHistory", id);

                // Check if Part exists (if PartId is provided)
                if (payload.PartId.HasValue && payload.PartId != Guid.Empty)
                {
                    var partExists = await _context.VehicleParts.AnyAsync(p => p.PartId == payload.PartId.Value, cancellationToken);
                    if (!partExists)
                        throw new BadRequestException($"VehiclePart with ID {payload.PartId} not found");
                }

                // Parse string enums to enum types
                if (!Enum.TryParse<WarrantyEventType>(payload.EventType, true, out var eventType))
                    throw new BadRequestException($"Invalid EventType: {payload.EventType}. Valid values are: REPAIR, REPLACEMENT, INSPECTION, EXTENSION");

                if (!Enum.TryParse<WarrantyHistoryStatus>(payload.Status, true, out var status))
                    throw new BadRequestException($"Invalid Status: {payload.Status}. Valid values are: Active, Completed, Cancelled, Deleted");

                existing.PartId = payload.PartId;
                existing.ClaimId = payload.ClaimId;
                existing.PolicyId = payload.PolicyId;
                existing.EventType = eventType;
                existing.Description = payload.Description;
                existing.PerformedBy = payload.PerformedBy;
                existing.ServiceCenterName = payload.ServiceCenterName;
                existing.WarrantyStartDate = payload.WarrantyStartDate;
                existing.WarrantyEndDate = payload.WarrantyEndDate;
                existing.Status = status;
                existing.UpdatedAt = DateTime.UtcNow;

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
                throw new InternalServerException("An error occurred while updating the warranty history", ex.Message);
            }
        }

        // DELETE: /api/warrantyhistories/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new BadRequestException("WarrantyHistory ID is required");

                var ok = await _repository.SoftDeleteAsync(id, cancellationToken);
                if (!ok)
                    throw new NotFoundException("WarrantyHistory", id);

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
                throw new InternalServerException("An error occurred while deleting the warranty history", ex.Message);
            }
        }
    }
}
