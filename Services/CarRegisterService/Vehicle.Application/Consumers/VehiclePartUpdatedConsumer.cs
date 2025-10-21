using BuildingBlocks.Messaging.Events;
using MassTransit;
using Vehicle.Application.Repositories;
using Vehicle.Domain.Enums;


namespace Vehicle.Application.Consumers
{
    public class VehiclePartUpdatedConsumer : IConsumer<VehiclePartUpdatedEvent>
    {
        private readonly IVehiclePartRepository _vehiclePartRepository;
        private readonly ILogger<VehiclePartUpdatedConsumer> _logger;

        public VehiclePartUpdatedConsumer(
            IVehiclePartRepository vehiclePartRepository,
            ILogger<VehiclePartUpdatedConsumer> logger)
        {
            _vehiclePartRepository = vehiclePartRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<VehiclePartUpdatedEvent> context)
        {
            var message = context.Message;
            
            try
            {
                _logger.LogInformation("Received VehiclePartUpdatedEvent for PartId {PartId}", message.PartId);

                // Get existing vehicle part
                var existingPart = await _vehiclePartRepository.GetByIdAsync(message.PartId, context.CancellationToken);
                
                if (existingPart == null)
                {
                    _logger.LogWarning("Vehicle part {PartId} not found, skipping update", message.PartId);
                    return;
                }

                // Update fields from event
                existingPart.SerialNumber = message.SerialNumber;
                existingPart.Status = Enum.TryParse<PartStatus>(message.Status, true, out var status) 
                    ? status 
                    : existingPart.Status;

                // Update additional fields if provided
                if (!string.IsNullOrEmpty(message.Description))
                {
                    // Note: VehiclePart entity might need Description field added
                    // For now, we'll skip this if the field doesn't exist
                }

                await _vehiclePartRepository.UpdateAsync(existingPart, context.CancellationToken);
                
                _logger.LogInformation("Successfully updated vehicle part {PartId}", message.PartId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing VehiclePartUpdatedEvent for PartId {PartId}", message.PartId);
                throw; // Re-throw to trigger retry mechanism
            }
        }
    }
}
