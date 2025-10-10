using MediatR;
using Microsoft.EntityFrameworkCore;
using PartCatalog.Application.Data;

namespace PartCatalog.Application.CQRS.Commands.UpdateWarrantyPolicy
{
    public class UpdateWarrantyPolicyHandler : IRequestHandler<UpdateWarrantyPolicyCommand, UpdateWarrantyPolicyResult>
    {
        private readonly IApplicationDbContext _context;

        public UpdateWarrantyPolicyHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UpdateWarrantyPolicyResult> Handle(UpdateWarrantyPolicyCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Policy;

            var entity = await _context.WarrantyPolicies
                .FirstOrDefaultAsync(p => p.PolicyId == dto.PolicyId, cancellationToken);

            if (entity == null)
            {
                return new UpdateWarrantyPolicyResult(false, "Warranty Policy not found.");
            }

            // Cập nhật thông tin
            entity.Code = dto.Code;
            entity.Name = dto.Name;
            entity.Type = dto.Type;
            entity.Status = dto.Status;
            entity.Description = dto.Description;
            entity.WarrantyDuration = dto.WarrantyDuration;
            entity.WarrantyDistance = dto.WarrantyDistance;
            entity.PackageId = dto.PackageId;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return new UpdateWarrantyPolicyResult(true, "Warranty Policy updated successfully.");
        }
    }
}
