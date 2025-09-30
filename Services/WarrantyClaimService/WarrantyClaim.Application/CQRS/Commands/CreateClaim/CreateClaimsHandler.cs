using Microsoft.EntityFrameworkCore;
using WarrantyClaim.Application.CQRS.Commands.CreateClaim;
using WarrantyClaim.Application.Data;
using WarrantyClaim.Domain.Enums;
using WarrantyClaim.Domain.Models;

namespace WarrantyClaim.Application.Commands
{
    public class CreateClaimsHandler
        : ICommandHandler<CreateClaimsCommand, CreateClaimsResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateClaimsHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

       

        public async Task<CreateClaimsResult> Handle(CreateClaimsCommand request, CancellationToken cancellationToken)
        {
            var claimId = Guid.NewGuid();

            var claim = _mapper.Map<Claim>(request.Claim);
            claim.Id = claimId;

            claim.Items = request.Claim.Items.Select(i =>
            {
                var item = _mapper.Map<ClaimItem>(i);
                item.Id = Guid.NewGuid();
                item.ClaimId = claimId;
                return item;
            }).ToList();

            if (claim.TotalPrice is null)
            {
                claim.TotalPrice = claim.Items
                    .Where(x => x.PayAmount.HasValue)
                    .Sum(x => x.PayAmount!.Value);
            }

            _context.Claims.Add(claim);
            await _context.SaveChangesAsync(cancellationToken);

            return new CreateClaimsResult(claim.Id);
        }
    }
}
