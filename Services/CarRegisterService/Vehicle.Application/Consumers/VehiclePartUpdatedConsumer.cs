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
        private readonly ILogger<VehiclePartUpdatedConsumer> _logger;

        public VehiclePartUpdatedConsumer(
            IVehiclePartRepository vehiclePartRepository,
            IWarrantyHistoryRepository warrantyHistoryRepository,
            ILogger<VehiclePartUpdatedConsumer> logger)
        {
            _vehiclePartRepository = vehiclePartRepository;
            _warrantyHistoryRepository = warrantyHistoryRepository;
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
                await SaveOldPartToWarrantyHistory(existingPart, message, context.CancellationToken);

                // Update VehiclePart với thông tin mới
                existingPart.SerialNumber = message.SerialNumber;
                existingPart.Status = PartStatus.Installed; // Force status to Installed
                existingPart.InstalledAt = DateTime.UtcNow;

                // Tính toán warranty dates và distance từ WarrantyPolicy
                await CalculateWarrantyInfo(existingPart, message, context.CancellationToken);

                await _vehiclePartRepository.UpdateAsync(existingPart, context.CancellationToken);
                
                _logger.LogInformation("Successfully updated vehicle part {PartId} with new serial {NewSerial}", 
                    message.PartId, message.SerialNumber);
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

        private async Task CalculateWarrantyInfo(VehiclePart part, VehiclePartUpdatedEvent message, CancellationToken cancellationToken)
        {
            try
            {
                // TODO: Implement logic to get WarrantyPolicy from PartCatalogService
                // For now, set default values
                part.WarrantyStartDate = DateTime.UtcNow;
                part.WarrantyEndDate = DateTime.UtcNow.AddMonths(12); // Default 12 months
                part.WarrantyDistance = 50000; // Default 50,000 km

                _logger.LogInformation("Calculated warranty info for PartId {PartId}: Start={StartDate}, End={EndDate}, Distance={Distance}km", 
                    message.PartId, part.WarrantyStartDate, part.WarrantyEndDate, part.WarrantyDistance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to calculate warranty info for PartId {PartId}", message.PartId);
                // Set default values if calculation fails
                part.WarrantyStartDate = DateTime.UtcNow;
                part.WarrantyEndDate = DateTime.UtcNow.AddMonths(12);
                part.WarrantyDistance = 50000;
            }
        }
    }
}