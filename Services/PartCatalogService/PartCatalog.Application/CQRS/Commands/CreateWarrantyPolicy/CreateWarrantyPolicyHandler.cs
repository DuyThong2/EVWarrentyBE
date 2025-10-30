using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PartCatalog.Application.CQRS.Commands.CreateWarrantyPolicy;
using PartCatalog.Application.Data;
using PartCatalog.Domain.Models;

namespace PartCatalog.Application.CQRS.Commands.CreateWarrantyPolicy
{
    public class CreateWarrantyPolicyHandler : IRequestHandler<CreateWarrantyPolicyCommand, CreateWarrantyPolicyResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateWarrantyPolicyHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CreateWarrantyPolicyResult> Handle(CreateWarrantyPolicyCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Policy;

            // Check package existence
            var package = await _context.Packages
                .FirstOrDefaultAsync(p => p.PackageId == dto.PackageId, cancellationToken);

            if (package == null)
                return new CreateWarrantyPolicyResult(Guid.Empty, false, "Package not found.");

            // Check duplicate code
            var duplicate = await _context.WarrantyPolicies
                .AnyAsync(p => p.Code == dto.Code, cancellationToken);

            if (duplicate)
                return new CreateWarrantyPolicyResult(Guid.Empty, false, "Policy code already exists.");

            // Map and add entity
            var entity = _mapper.Map<WarrantyPolicy>(dto);
            entity.PolicyId = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;

            await _context.WarrantyPolicies.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new CreateWarrantyPolicyResult(entity.PolicyId, true, "Warranty policy created successfully.");
        }
    }
}
