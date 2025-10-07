using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarrantyClaim.Application.Exceptions;

namespace WarrantyClaim.Application.CQRS.Commands.UpdateClaim
{
    internal class UpdateClaimHandler
        : ICommandHandler<UpdateClaimCommand, UpdateClaimResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateClaimHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UpdateClaimResult> Handle(
           UpdateClaimCommand request,
           CancellationToken cancellationToken)
        {
            var dto = request.Claim;

            var claim = await _context.Claims
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == dto.Id, cancellationToken);

            if (claim is null)
                throw new ClaimNotFoundException(dto.Id);

            UpdateClaimScalar(dto, claim);

            UpsertClaimItems(dto, claim);

            await _context.SaveChangesAsync(cancellationToken);

            return new UpdateClaimResult(true);
        }

        
        private void UpdateClaimScalar(ClaimDto dto, Claim claim)
        {
            _mapper.Map(dto, claim);
        }


        private void UpsertClaimItems(ClaimDto dto, Claim claim)
        {
            if (dto.Items is null) return;

            var existingById = claim.Items.ToDictionary(i => i.Id, i => i);

            foreach (var itemDto in dto.Items)
            {
                if (itemDto.Id != Guid.Empty &&
                    existingById.TryGetValue(itemDto.Id, out var exist))
                {
                    _mapper.Map(itemDto, exist);
                }
                else
                {
                    var newItem = _mapper.Map<ClaimItem>(itemDto);
                    newItem.Id = Guid.NewGuid();
                    newItem.ClaimId = claim.Id;
                    claim.Items.Add(newItem);
                }
            }

            var dtoIds = dto.Items.Where(i => i.Id != Guid.Empty).Select(i => i.Id).ToHashSet();
            var toRemove = claim.Items.Where(i => !dtoIds.Contains(i.Id)).ToList();

            foreach (var item in toRemove)
            {
                claim.Items.Remove(item);
            }

            if (dto.TotalPrice is null)
            {
                claim.TotalPrice = claim.Items
                    .Where(i => i.PayAmount.HasValue)
                    .Sum(i => i.PayAmount!.Value);
            }
        }
    }

}
