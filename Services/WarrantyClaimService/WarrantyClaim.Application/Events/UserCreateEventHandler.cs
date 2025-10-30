// WarrantyClaim.Application/Events/UserCreateEventHandler.cs
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Messaging.Events.UserEvent;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WarrantyClaim.Application.CQRS.Commands.CreateTechician;

using WarrantyClaim.Domain.Enums; // nơi có TechnicianStatus
using MediatR; // ISender

namespace WarrantyClaim.Application.Events
{
    // Nhận TechnicianUpsertEvent để "upsert" Technician theo StaffId
    internal class UserCreateEventHandler : IConsumer<TechnicianUpsertEvent>
    {
        private readonly ILogger<UserCreateEventHandler> _logger;
        private readonly IApplicationDbContext _context;
        private readonly ISender _sender; // gửi CreateTechnicianCommand

        public UserCreateEventHandler(
            ILogger<UserCreateEventHandler> logger,
            IApplicationDbContext context,
            ISender sender)
        {
            _logger = logger;
            _context = context;
            _sender = sender;
        }

        public async Task Consume(ConsumeContext<TechnicianUpsertEvent> context)
        {
            var e = context.Message;

            _logger.LogInformation(
                "[TechUpsertEvent] StaffId={StaffId}, FullName={FullName}, Email={Email}, Phone={Phone}, Status={Status}",
                e.StaffId, e.FullName, e.Email, e.Phone, e.Status);

            // Tìm theo StaffId (id của User)
            var existing = await _context.Technicians
                .FirstOrDefaultAsync(t => t.StaffId == e.StaffId, context.CancellationToken);

            if (existing is null)
            {
                // → tạo mới qua CQRS command (xài validator + map sẵn của bạn)
                var dto = new TechnicianDto
                {
                    // TechnicianDto nên có các field sau; nếu tên khác, sửa lại cho khớp
                    StaffId = e.StaffId,
                    FullName = e.FullName,
                    Email = e.Email,
                    Phone = e.Phone,
                    Status = e.Status // để validator của bạn xử lý string trước; mapping sẽ parse sang enum
                };

                var cmd = new CreateTechnicianCommand(dto);
                var result = await _sender.Send(cmd, context.CancellationToken);

                _logger.LogInformation("[TechUpsertEvent] Created TechnicianId={TechnicianId} for StaffId={StaffId}",
                    result.TechnicianId, e.StaffId);
                return;
            }

            // → đã có: cập nhật trực tiếp
            existing.FullName = e.FullName?.Trim() ?? existing.FullName;
            existing.Email = string.IsNullOrWhiteSpace(e.Email) ? null : e.Email.Trim();
            existing.Phone = string.IsNullOrWhiteSpace(e.Phone) ? null : e.Phone.Trim();
            existing.Status = ParseStatus(e.Status);

            await _context.SaveChangesAsync(context.CancellationToken);

            _logger.LogInformation("[TechUpsertEvent] Updated Technician(Id={Id}) mapped from StaffId={StaffId}",
                existing.Id, e.StaffId);
        }

        private static TechnicianStatus ParseStatus(string? s)
        {
            if (string.Equals(s, "ACTIVE", System.StringComparison.OrdinalIgnoreCase))
                return TechnicianStatus.ACTIVE;
            return TechnicianStatus.INACTIVE;
        }
    }
}
