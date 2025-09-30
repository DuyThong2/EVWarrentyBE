using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Commands.DeleteClaim
{
    internal class DeleteClaimHandler
        : ICommandHandler<DeleteClaimCommand, DeleteClaimResult>
    {
        private readonly IApplicationDbContext _context;

        public DeleteClaimHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DeleteClaimResult> Handle(
            DeleteClaimCommand request,
            CancellationToken cancellationToken)
        {
            var claim = await _context.Claims.FindAsync([request.ClaimId], cancellationToken);
            if (claim is null)
                throw new KeyNotFoundException($"Claim {request.ClaimId} not found.");

            _context.Claims.Remove(claim);
            await _context.SaveChangesAsync(cancellationToken);

            return new DeleteClaimResult(true);
        }
    }
}
