using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Commands.UpdateWorkOrder
{
    internal class UpdateWorkOrderHandler
        : ICommandHandler<UpdateWorkOrderCommand, UpdateWorkOrderResult>
    {
        private readonly IApplicationDbContext _context;

        public UpdateWorkOrderHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UpdateWorkOrderResult> Handle(
            UpdateWorkOrderCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.WorkOrder;

            // Load WorkOrder
            var wo = await _context.WorkOrders
                .FirstOrDefaultAsync(x => x.Id == dto.Id, cancellationToken);

            if (wo is null)
                throw new KeyNotFoundException($"WorkOrder {dto.Id} not found.");

            // Update scalar fields
            wo.ClaimItemId = dto.ClaimItemId;   // nếu không muốn cho phép "move" thì bỏ dòng này
            wo.TechnicianId = dto.TechnicianId;
            wo.WorkingHours = dto.WorkingHours;
            wo.StartedAt = dto.StartedAt;
            wo.EndDate = dto.EndDate;

            // Parse status string -> enum; nếu null/rỗng thì giữ nguyên
            if (!string.IsNullOrWhiteSpace(dto.Status) &&
                Enum.TryParse<WorkOrderStatus>(dto.Status, true, out var parsed))
            {
                wo.Status = parsed;
            }

            // Không set Technician navigation ở đây (theo yêu cầu)
            await _context.SaveChangesAsync(cancellationToken);

            return new UpdateWorkOrderResult(true);
        }
    }
}
