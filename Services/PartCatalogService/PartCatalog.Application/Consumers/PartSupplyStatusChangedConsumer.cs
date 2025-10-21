using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using BuildingBlocks.Messaging.Events;
using PartCatalog.Application.Data;
using PartCatalog.Domain.Enums;

namespace PartCatalog.Application.Consumers
{
    public class PartSupplyStatusChangedConsumer
        : IConsumer<PartSupplyStatusChangedEvent>
    {
        private readonly IApplicationDbContext _context;

        public PartSupplyStatusChangedConsumer(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<PartSupplyStatusChangedEvent> context)
        {
            var message = context.Message;

            if (message.PartId == Guid.Empty)
                return; // tránh lỗi khi Id không hợp lệ

            var part = await _context.Parts
                .FirstOrDefaultAsync(p => p.PartId == message.PartId);

            if (part == null)
                return; // có thể log warning nếu cần

            // Map trạng thái từ SupplyStatus → ActiveStatus
            part.Status = message.NewStatus switch
            {
                "DELIVERED" => ActiveStatus.ACTIVE,
                "CANCELED" => ActiveStatus.INACTIVE,
                _ => part.Status // giữ nguyên nếu không nằm trong danh sách trên
            };

            await _context.SaveChangesAsync(context.CancellationToken);
        }
    }
}
