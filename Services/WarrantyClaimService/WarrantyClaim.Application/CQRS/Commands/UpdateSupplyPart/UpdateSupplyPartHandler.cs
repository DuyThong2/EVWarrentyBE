using BuildingBlocks.Messaging.Events;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Commands.UpdateSupplyPart
{
    internal class UpdateSupplyPartHandler
        : ICommandHandler<UpdateSupplyPartCommand, UpdateSupplyPartResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;

        public UpdateSupplyPartHandler(IApplicationDbContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
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

            return new UpdateSupplyPartResult(true);
        }
    }
}
