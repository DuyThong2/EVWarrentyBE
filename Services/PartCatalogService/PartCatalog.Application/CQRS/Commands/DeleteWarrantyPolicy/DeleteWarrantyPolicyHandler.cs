using MediatR;
using Microsoft.EntityFrameworkCore;
using PartCatalog.Application.CQRS.Commands.DeleteWarrantyPolicy;
using PartCatalog.Application.Data;

namespace PartCatalog.Application.Features.WarrantyPolicies.Handlers
{
    public class DeleteWarrantyPolicyHandler : IRequestHandler<DeleteWarrantyPolicyCommand, DeleteWarrantyPolicyResult>
    {
        private readonly IApplicationDbContext _context;

        public DeleteWarrantyPolicyHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DeleteWarrantyPolicyResult> Handle(DeleteWarrantyPolicyCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.WarrantyPolicies
                .FirstOrDefaultAsync(x => x.PolicyId == request.PolicyId, cancellationToken);

            if (entity == null)
            {
                return new DeleteWarrantyPolicyResult(false, "Warranty Policy not found.");
            }

            _context.WarrantyPolicies.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return new DeleteWarrantyPolicyResult(true, "Warranty Policy deleted successfully.");
        }
    }
}
