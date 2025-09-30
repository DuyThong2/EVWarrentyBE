using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Commands.CreateWorkOrder
{
    internal class CreateWorkOrderHandler
        : ICommandHandler<CreateWorkOrderCommand, CreateWorkOrderResult>
    {
        private readonly IApplicationDbContext _context;

        public CreateWorkOrderHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CreateWorkOrderResult> Handle(
            CreateWorkOrderCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.WorkOrder;

            // (Tuỳ chọn) xác minh ClaimItem tồn tại
            // var exists = await _context.ClaimItems.AnyAsync(i => i.Id == dto.ClaimItemId, cancellationToken);
            // if (!exists) throw new KeyNotFoundException($"ClaimItem {dto.ClaimItemId} not found.");

            // Parse Status string -> enum; default NEW nếu null/rỗng/invalid
            var status = WorkOrderStatus.OPEN;
            if (!string.IsNullOrWhiteSpace(dto.Status) &&
                Enum.TryParse<WorkOrderStatus>(dto.Status, true, out var parsed))
            {
                status = parsed;
            }

            var workOrder = new WorkOrder
            {
                Id = Guid.NewGuid(),
                ClaimItemId = dto.ClaimItemId,
                TechnicianId = dto.TechnicianId,
                WorkingHours = dto.WorkingHours,
                Status = status,
                StartedAt = dto.StartedAt,
                EndDate = dto.EndDate
            };

            _context.WorkOrders.Add(workOrder);
            await _context.SaveChangesAsync(cancellationToken);

            return new CreateWorkOrderResult(workOrder.Id);
        }
    }
}
