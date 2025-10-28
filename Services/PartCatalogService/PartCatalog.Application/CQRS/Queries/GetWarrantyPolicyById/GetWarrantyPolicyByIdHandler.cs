using MediatR;
using Microsoft.EntityFrameworkCore;
using PartCatalog.Application.CQRS.Queries.GetWarrantyPolicyById;
using PartCatalog.Application.Data;
using PartCatalog.Application.DTOs;
using AutoMapper;

namespace PartCatalog.Application.Features.WarrantyPolicies.Handlers
{
    public class GetWarrantyPolicyByIdHandler
        : IRequestHandler<GetWarrantyPolicyByIdQuery, GetWarrantyPolicyByIdResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetWarrantyPolicyByIdHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetWarrantyPolicyByIdResult> Handle(GetWarrantyPolicyByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.WarrantyPolicies
                .Include(p => p.Package)
                    .ThenInclude(pkg => pkg.Category)
                .FirstOrDefaultAsync(p => p.PolicyId == request.PolicyId, cancellationToken);

            if (entity == null)
                throw new KeyNotFoundException($"WarrantyPolicy with Id {request.PolicyId} not found.");

            var dto = _mapper.Map<WarrantyPolicyDto>(entity);

            return new GetWarrantyPolicyByIdResult(dto);
        }
    }
}
