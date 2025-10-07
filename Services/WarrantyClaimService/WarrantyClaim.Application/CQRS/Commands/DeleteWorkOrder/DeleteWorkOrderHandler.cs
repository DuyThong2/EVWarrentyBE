using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Commands.DeleteWorkOrder
{
    internal class DeleteWorkOrderHandler
        : ICommandHandler<DeleteWorkOrderCommand, DeleteWorkOrderResult>
    {
        private readonly IApplicationDbContext _context;

        public DeleteWorkOrderHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DeleteWorkOrderResult> Handle(
            DeleteWorkOrderCommand request,
            CancellationToken cancellationToken)
        {
            var workOrder = await _context.WorkOrders.FindAsync([request.WorkOrderId], cancellationToken);
            if (workOrder is null)
                throw new KeyNotFoundException($"WorkOrder {request.WorkOrderId} not found.");

            _context.WorkOrders.Remove(workOrder);
            await _context.SaveChangesAsync(cancellationToken);

            return new DeleteWorkOrderResult(true);
        }
    }
}
