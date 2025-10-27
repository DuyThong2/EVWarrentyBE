using MassTransit;
using BuildingBlocks.Messaging.Events;
using PartCatalog.Application.Data;
using Microsoft.EntityFrameworkCore;

namespace PartCatalog.Application.Consumers
{
    public class WarrantyPolicyRequestedConsumer : IConsumer<WarrantyPolicyRequestedEvent>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<WarrantyPolicyRequestedConsumer> _logger;

        public WarrantyPolicyRequestedConsumer(
            IApplicationDbContext context,
            ILogger<WarrantyPolicyRequestedConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<WarrantyPolicyRequestedEvent> context)
        {
            var message = context.Message;
            
            try
            {
                _logger.LogInformation("=== START WarrantyPolicyRequestedEvent === SerialNumber: {SerialNumber}, Code: {Code}, RequestId: {RequestId}, PartId: {PartId}", 
                    message.SerialNumber, message.Code, message.RequestId, message.PartId);

                // Find WarrantyPolicy by Code directly
                var warrantyPolicy = await _context.WarrantyPolicies
                    .FirstOrDefaultAsync(wp => wp.Code == message.Code, context.CancellationToken);
                
                if (warrantyPolicy == null)
                {
                    _logger.LogWarning("WarrantyPolicy not found for Code {Code}", message.Code);
                    
                    // Send error response
                    var errorResponse = new WarrantyPolicyResponseEvent
                    {
                        RequestId = message.RequestId,
                        PartId = message.PartId,
                        WarrantyDuration = 0,
                        WarrantyDistance = 0,
                        IsSuccess = false,
                        ErrorMessage = $"WarrantyPolicy not found for Code {message.Code}"
                    };
                    
                    await context.Publish(errorResponse, context.CancellationToken);
                    return;
                }

                // Send success response
                var successResponse = new WarrantyPolicyResponseEvent
                {
                    RequestId = message.RequestId,
                    PartId = message.PartId,
                    WarrantyDuration = warrantyPolicy.WarrantyDuration ?? 0,
                    WarrantyDistance = (int)(warrantyPolicy.WarrantyDistance ?? 0),
                    IsSuccess = true,
                    ErrorMessage = null
                };

                await context.Publish(successResponse, context.CancellationToken);
                
                _logger.LogInformation("✅ Successfully sent WarrantyPolicyResponseEvent - RequestId: {RequestId}, Duration={Duration}months, Distance={Distance}km", 
                    message.RequestId, successResponse.WarrantyDuration, successResponse.WarrantyDistance);
                
                _logger.LogInformation("=== COMPLETED WarrantyPolicyRequestedEvent === RequestId: {RequestId}", message.RequestId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing WarrantyPolicyRequestedEvent for SerialNumber {SerialNumber}", message.SerialNumber);
                
                // Send error response
                var errorResponse = new WarrantyPolicyResponseEvent
                {
                    RequestId = message.RequestId,
                    PartId = message.PartId,
                    WarrantyDuration = 0,
                    WarrantyDistance = 0,
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
                
                await context.Publish(errorResponse, context.CancellationToken);
            }
        }
    }
}
