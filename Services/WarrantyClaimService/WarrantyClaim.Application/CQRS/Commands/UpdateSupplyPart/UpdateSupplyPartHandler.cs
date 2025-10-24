using BuildingBlocks.Messaging.Events;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WarrantyClaim.Application.CQRS.Commands.UpdateSupplyPart
{
    internal class UpdateSupplyPartHandler
        : ICommandHandler<UpdateSupplyPartCommand, UpdateSupplyPartResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateSupplyPartHandler> _logger;

        public UpdateSupplyPartHandler(
            IApplicationDbContext context,
            IPublishEndpoint publishEndpoint,
            IMapper mapper,
            ILogger<UpdateSupplyPartHandler> logger)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UpdateSupplyPartResult> Handle(
            UpdateSupplyPartCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.SupplyPart;

            // Load entity
            var partSupply = await _context.PartSupplies
                .FirstOrDefaultAsync(p => p.Id == dto.Id, cancellationToken);

            if (partSupply is null)
                throw new KeyNotFoundException($"PartSupply {dto.Id} not found.");

            // Update scalar fields
            partSupply.ClaimItemId = dto.ClaimItemId;
            partSupply.PartId = dto.PartId;
            partSupply.Description = dto.Description;
            partSupply.NewSerialNumber = dto.NewSerialNumber;
            partSupply.OldPartSerialNumber = dto.OldPartSerialNumber; // Thêm field mới
            partSupply.ShipmentCode = dto.ShipmentCode;
            partSupply.ShipmentRef = dto.ShipmentRef;

            // Parse status nếu có
            if (!string.IsNullOrWhiteSpace(dto.Status) &&
                Enum.TryParse<SupplyStatus>(dto.Status, true, out var parsed))
            {
                partSupply.Status = parsed;
            }

            await _context.SaveChangesAsync(cancellationToken);

            // ✅ Chỉ publish khi Status = INSTALLED và có PartId
            if (dto.PartId.HasValue && dto.PartId.Value != Guid.Empty &&
                partSupply.Status == SupplyStatus.INSTALLED)
            {
                try
                {
                    // Lấy thông tin Claim và StaffId từ ClaimItem
                    var claimItem = await _context.ClaimItems
                        .Include(ci => ci.Claim)
                        .FirstOrDefaultAsync(ci => ci.Id == dto.ClaimItemId, cancellationToken);

                    var vehiclePartUpdatedEvent = new VehiclePartUpdatedEvent
                    {
                        PartId = dto.PartId.Value,
                        SerialNumber = dto.NewSerialNumber ?? string.Empty,
                        OldPartSerialNumber = dto.OldPartSerialNumber,
                        Status = "INSTALLED", // Force status to INSTALLED
                        Description = dto.Description,
                        ShipmentCode = dto.ShipmentCode,
                        ShipmentRef = dto.ShipmentRef,
                        ClaimId = claimItem?.Claim?.Id,
                        PerformedBy = claimItem?.Claim?.StaffId
                    };

                    await _publishEndpoint.Publish(vehiclePartUpdatedEvent, cancellationToken);
                    _logger.LogInformation("✅ Published VehiclePartUpdatedEvent for PartId {PartId} with INSTALLED status", dto.PartId.Value);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Failed to publish VehiclePartUpdatedEvent for PartId {PartId}", dto.PartId.Value);
                }
            }
            else
            {
                _logger.LogInformation("ℹ️ SupplyPart {PartId} updated but status is not INSTALLED. Skipped event publish. Current status: {Status}",
                    dto.PartId.Value, partSupply.Status);
            }

            return new UpdateSupplyPartResult(true);
        }
    }
}
