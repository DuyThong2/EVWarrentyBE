using BuildingBlocks.Messaging.Events;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BuildingBlocks.Messaging.Events;
using MassTransit;

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

            var oldStatus = partSupply.Status;

            // Update scalar fields
            partSupply.ClaimItemId = dto.ClaimItemId;
            partSupply.PartId = dto.PartId;
            partSupply.Description = dto.Description;
            partSupply.NewSerialNumber = dto.NewSerialNumber;
            partSupply.ShipmentCode = dto.ShipmentCode;
            partSupply.ShipmentRef = dto.ShipmentRef;

            if (!string.IsNullOrWhiteSpace(dto.Status) &&
                Enum.TryParse<SupplyStatus>(dto.Status, true, out var parsed))
            {
                partSupply.Status = parsed;
            }

         
            await _context.SaveChangesAsync(cancellationToken);

            // Publish event nếu Status thay đổi
            if (partSupply.Status != oldStatus)
            {
                var @event = new PartSupplyStatusChangedEvent(
                    partSupply.PartId.Value,
                    partSupply.Status.ToString()
                );

                await _publishEndpoint.Publish(@event, cancellationToken);
            }

            
            if (dto.PartId.HasValue && dto.PartId.Value != Guid.Empty)
            {
                try
                {
                    var vehiclePartUpdatedEvent = new VehiclePartUpdatedEvent
                    {
                        PartId = dto.PartId.Value,
                        SerialNumber = dto.NewSerialNumber ?? string.Empty,
                        Status = dto.Status ?? "Installed",
                        Description = dto.Description,
                        ShipmentCode = dto.ShipmentCode,
                        ShipmentRef = dto.ShipmentRef,
                      
                    };

                    await _publishEndpoint.Publish(vehiclePartUpdatedEvent, cancellationToken);
                    _logger.LogInformation("Published VehiclePartUpdatedEvent for PartId {PartId}", dto.PartId.Value);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to publish VehiclePartUpdatedEvent for PartId {PartId}", dto.PartId.Value);
                  
                }
            }

            return new UpdateSupplyPartResult(true);
        }
    }
}
