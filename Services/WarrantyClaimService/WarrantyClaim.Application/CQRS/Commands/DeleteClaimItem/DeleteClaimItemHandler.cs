using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Commands.DeleteClaimItem
{
    internal class DeleteClaimItemHandler
        : ICommandHandler<DeleteClaimItemCommand, DeleteClaimItemResult>
    {
        private readonly IApplicationDbContext _context;

        public DeleteClaimItemHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DeleteClaimItemResult> Handle(
            DeleteClaimItemCommand request,
            CancellationToken cancellationToken)
        {
            // Cách 1: Find rồi Remove
            var item = await _context.ClaimItems.FindAsync([request.ClaimItemId], cancellationToken);
            if (item is null)
                throw new KeyNotFoundException($"ClaimItem {request.ClaimItemId} not found.");

            _context.ClaimItems.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);

            return new DeleteClaimItemResult(true);
        }
    }
}
