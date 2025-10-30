using MassTransit;
using BuildingBlocks.Messaging.Events;
using Vehicle.Application.Repositories;
using Vehicle.Domain.Models;

namespace Vehicle.Application.Consumers
{
    public class WarrantyPolicyResponseConsumer : IConsumer<WarrantyPolicyResponseEvent>
    {
        private readonly IVehiclePartRepository _vehiclePartRepository;
        private readonly ILogger<WarrantyPolicyResponseConsumer> _logger;

        public WarrantyPolicyResponseConsumer(
            IVehiclePartRepository vehiclePartRepository,
            ILogger<WarrantyPolicyResponseConsumer> logger)
        {
            _vehiclePartRepository = vehiclePartRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<WarrantyPolicyResponseEvent> context)
        {
            var message = context.Message;
            
            try
            {
                _logger.LogInformation("=== START WarrantyPolicyResponseEvent === RequestId: {RequestId}, PartId: {PartId}, Success: {Success}", 
                    message.RequestId, message.PartId, message.IsSuccess);

                if (!message.IsSuccess)
                {
                    _logger.LogWarning("WarrantyPolicyResponseEvent failed for PartId {PartId}: {ErrorMessage}", 
                        message.PartId, message.ErrorMessage);
                    return;
                }

                // Get the vehicle part
                var vehiclePart = await _vehiclePartRepository.GetByIdAsync(message.PartId, context.CancellationToken);
                
                if (vehiclePart == null)
                {
                    _logger.LogWarning("VehiclePart {PartId} not found for warranty policy update", message.PartId);
                    return;
                }

                // Cộng warranty information với policy data từ PartCatalogService
                var currentWarrantyEndDate = vehiclePart.WarrantyEndDate ?? DateTime.UtcNow;
                var currentWarrantyDistance = vehiclePart.WarrantyDistance ?? 0;
                
                vehiclePart.WarrantyStartDate = DateTime.UtcNow; // Reset start date cho part mới
                vehiclePart.WarrantyEndDate = currentWarrantyEndDate.AddMonths(message.WarrantyDuration); // Cộng thêm duration
                vehiclePart.WarrantyDistance = currentWarrantyDistance + message.WarrantyDistance; // Cộng thêm distance

                await _vehiclePartRepository.UpdateAsync(vehiclePart, context.CancellationToken);

                _logger.LogInformation("✅ Successfully added warranty info for PartId {PartId}: Added Duration={Duration}months, Added Distance={Distance}km, New EndDate={EndDate}, New Total Distance={TotalDistance}km", 
                    message.PartId, message.WarrantyDuration, message.WarrantyDistance, vehiclePart.WarrantyEndDate, vehiclePart.WarrantyDistance);
                
                _logger.LogInformation("=== COMPLETED WarrantyPolicyResponseEvent === RequestId: {RequestId}", message.RequestId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing WarrantyPolicyResponseEvent for PartId {PartId}", message.PartId);
                // Don't throw - this is not critical for the main flow
            }
        }
    }
}
