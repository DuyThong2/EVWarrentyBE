using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Commands.CreateClaimItem
{
    internal class CreateClaimItemHandler
        : ICommandHandler<CreateClaimItemCommand, CreateClaimItemResult>
    {
        private readonly IApplicationDbContext _context;

        public CreateClaimItemHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CreateClaimItemResult> Handle(
            CreateClaimItemCommand request,
            CancellationToken cancellationToken)
        {
            var claim = await _context.Claims
                .FirstOrDefaultAsync(c => c.Id == request.ClaimId, cancellationToken);

            if (claim is null)
                throw new KeyNotFoundException($"Claim {request.ClaimId} not found.");

            var dto = request.Item;

            var status = ClaimItemStatus.PENDING;
            if (!string.IsNullOrWhiteSpace(dto.Status) &&
                Enum.TryParse<ClaimItemStatus>(dto.Status, true, out var parsed))
            {
                status = parsed;
            }

            var newItem = new ClaimItem
            {
                Id = Guid.NewGuid(),
                ClaimId = request.ClaimId,
                PartSerialNumber = dto.PartSerialNumber,
                PayAmount = dto.PayAmount,
                PaidBy = dto.PaidBy,
                Note = dto.Note,
                ImgURLs = dto.ImgURLs,
                Status = status
            };

            _context.ClaimItems.Add(newItem);
            await _context.SaveChangesAsync(cancellationToken);

            return new CreateClaimItemResult(newItem.Id);
        }
    }
}
