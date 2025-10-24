// WarrantyClaim.Application/Events/UserUpdatedEventHandler.cs
using System.Threading.Tasks;
using BuildingBlocks.Messaging.Events.UserEvent;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WarrantyClaim.Domain.Enums;

namespace WarrantyClaim.Application.Events
{
    // Nhận TechnicianDeletedEvent để vô hiệu hoá (hoặc xoá) Technician theo StaffId
    internal class UserUpdatedEventHandler : IConsumer<TechnicianDeletedEvent>
    {
        private readonly ILogger<UserUpdatedEventHandler> _logger;
        private readonly IApplicationDbContext _context;

        public UserUpdatedEventHandler(
            ILogger<UserUpdatedEventHandler> logger,
            IApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task Consume(ConsumeContext<TechnicianDeletedEvent> context)
        {
            var e = context.Message;

            _logger.LogInformation("[TechDeletedEvent] StaffId={StaffId}", e.StaffId);

            var existing = await _context.Technicians
                .FirstOrDefaultAsync(t => t.StaffId == e.StaffId, context.CancellationToken);

            if (existing is null)
            {
                // Idempotent: không có thì bỏ qua
                _logger.LogInformation("[TechDeletedEvent] No Technician found for StaffId={StaffId}. Skip.", e.StaffId);
                return;
            }

            // Tuỳ chính sách: Deactivate (khuyến nghị) hoặc hard delete
            existing.Status = TechnicianStatus.INACTIVE;

            await _context.SaveChangesAsync(context.CancellationToken);

            _logger.LogInformation("[TechDeletedEvent] Deactivated Technician(Id={Id}) for StaffId={StaffId}",
                existing.Id, e.StaffId);
        }
    }
}
