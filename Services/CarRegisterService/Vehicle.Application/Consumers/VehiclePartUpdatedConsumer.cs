using BuildingBlocks.Messaging.Events;
using MassTransit;
using Vehicle.Application.Repositories;
using Vehicle.Domain.Enums;
using Vehicle.Domain.Models;

namespace Vehicle.Application.Consumers
{
    public class VehiclePartUpdatedConsumer : IConsumer<VehiclePartUpdatedEvent>
    {
        private readonly IVehiclePartRepository _vehiclePartRepository;
        private readonly IWarrantyHistoryRepository _warrantyHistoryRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<VehiclePartUpdatedConsumer> _logger;

        public VehiclePartUpdatedConsumer(
            IVehiclePartRepository vehiclePartRepository,
            IWarrantyHistoryRepository warrantyHistoryRepository,
            IPublishEndpoint publishEndpoint,
            ILogger<VehiclePartUpdatedConsumer> logger)
        {
            _vehiclePartRepository = vehiclePartRepository;
            _warrantyHistoryRepository = warrantyHistoryRepository;
            _publishEndpoint = publishEndpoint;
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

                // Chỉ update khi OldPartSerialNumber khớp với SerialNumber hiện tại
                if (!string.IsNullOrEmpty(message.OldPartSerialNumber) && 
                    !message.OldPartSerialNumber.Equals(existingPart.SerialNumber, StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogWarning("OldPartSerialNumber {OldSerial} does not match current SerialNumber {CurrentSerial} for PartId {PartId}, skipping update", 
                        message.OldPartSerialNumber, existingPart.SerialNumber, message.PartId);
                    return;
                }

                // Lưu thông tin part cũ vào WarrantyHistory trước khi update
                _logger.LogInformation("STEP 1: Saving old part to warranty history - PartId: {PartId}, OldSerial: {OldSerial}", 
                    message.PartId, existingPart.SerialNumber);
                await SaveOldPartToWarrantyHistory(existingPart, message, context.CancellationToken);

                // Update VehiclePart với thông tin mới
                _logger.LogInformation("STEP 2: Updating part with new serial - PartId: {PartId}, NewSerial: {NewSerial}", 
                    message.PartId, message.SerialNumber);
                existingPart.SerialNumber = message.SerialNumber;
                existingPart.Status = PartStatus.Installed; // Force status to Installed
                existingPart.InstalledAt = DateTime.UtcNow;

                // Request warranty policy từ PartCatalogService để cộng vào warranty hiện tại
                _logger.LogInformation("STEP 3: Requesting warranty policy from PartCatalogService - PartId: {PartId}", message.PartId);
                await RequestWarrantyPolicy(existingPart, message, context.CancellationToken);

                _logger.LogInformation("STEP 4: Updating part in database - PartId: {PartId}", message.PartId);
                await _vehiclePartRepository.UpdateAsync(existingPart, context.CancellationToken);
                
                _logger.LogInformation("STEP 5: Saving new part to warranty history - PartId: {PartId}", message.PartId);
                await SaveNewPartToWarrantyHistory(existingPart, message, context.CancellationToken);


                _logger.LogInformation("=== COMPLETED VehiclePartUpdatedEvent === PartId: {PartId}", message.PartId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing VehiclePartUpdatedEvent for PartId {PartId}", message.PartId);
                throw; // Re-throw to trigger retry mechanism
            }
        }

        private async Task SaveOldPartToWarrantyHistory(VehiclePart oldPart, VehiclePartUpdatedEvent message, CancellationToken cancellationToken)
        {
            try
            {
                var warrantyHistory = new WarrantyHistory
                {
                    HistoryId = Guid.NewGuid(),
                    VehicleId = oldPart.VehicleId,
                    PartId = oldPart.PartId,
                    ClaimId = message.ClaimId,
                    EventType = WarrantyEventType.REPLACEMENT,
                    Description = $"Part replaced: {oldPart.SerialNumber} -> {message.SerialNumber}",
                    PerformedBy = message.PerformedBy,
                    WarrantyStartDate = oldPart.WarrantyStartDate,
                    WarrantyEndDate = oldPart.WarrantyEndDate,
                    WarrantyDistance = oldPart.WarrantyDistance,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Status = WarrantyHistoryStatus.Completed
                };

                await _warrantyHistoryRepository.AddAsync(warrantyHistory, cancellationToken);
                _logger.LogInformation("Saved old part {OldSerial} to warranty history for PartId {PartId}",
                    oldPart.SerialNumber, message.PartId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save old part to warranty history for PartId {PartId}", message.PartId);
                // Don't throw - this is not critical for the main flow
            }
        }

        private async Task SaveNewPartToWarrantyHistory(VehiclePart newPart, VehiclePartUpdatedEvent message, CancellationToken cancellationToken)
        {
            try
            {
                var newHistory = new WarrantyHistory
                {
                    HistoryId = Guid.NewGuid(),
                    VehicleId = newPart.VehicleId,
                    PartId = newPart.PartId,
                    ClaimId = message.ClaimId,
                    EventType = WarrantyEventType.INSTALLATION, // khác với REPLACEMENT của part cũ
                    Description = $"New part installed: {message.SerialNumber}",
                    PerformedBy = message.PerformedBy,
                    WarrantyStartDate = newPart.WarrantyStartDate,
                    WarrantyEndDate = newPart.WarrantyEndDate,
                    WarrantyDistance = newPart.WarrantyDistance,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Status = WarrantyHistoryStatus.Completed
                };

                await _warrantyHistoryRepository.AddAsync(newHistory, cancellationToken);
                _logger.LogInformation("Saved new part {NewSerial} to warranty history for PartId {PartId}",
                    newPart.SerialNumber, newPart.PartId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save new part to warranty history for PartId {PartId}", newPart.PartId);
                // Không throw, tránh ảnh hưởng main flow
            }
        }

        private async Task RequestWarrantyPolicy(VehiclePart part, VehiclePartUpdatedEvent message, CancellationToken cancellationToken)
        {
            try
            {
                var requestId = Guid.NewGuid();
                var warrantyRequest = new WarrantyPolicyRequestedEvent
                {
                    PartId = part.PartId,
                    SerialNumber = message.SerialNumber,
                    Code = part.Code,
                    RequestId = requestId
                };

                _logger.LogInformation("Publishing WarrantyPolicyRequestedEvent - PartId: {PartId}, SerialNumber: {SerialNumber}, Code: {Code}, RequestId: {RequestId}", 
                    part.PartId, message.SerialNumber, part.Code, requestId);
                
                await _publishEndpoint.Publish(warrantyRequest, cancellationToken);
                
                _logger.LogInformation("✅ Successfully published WarrantyPolicyRequestedEvent - RequestId: {RequestId}", requestId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Failed to publish WarrantyPolicyRequestedEvent for PartId {PartId}", message.PartId);
                // Không set default values ở đây - để WarrantyPolicyResponseConsumer xử lý
            }
        }
    }
}