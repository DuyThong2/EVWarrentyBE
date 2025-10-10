using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Commands.UpdateClaimItem
{
    internal class UpdateClaimItemHandler
        : ICommandHandler<UpdateClaimItemCommand, UpdateClaimItemResult>
    {
        private readonly IApplicationDbContext _context;

        public UpdateClaimItemHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UpdateClaimItemResult> Handle(
            UpdateClaimItemCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Item;

            // Load item cần update
            var item = await _context.ClaimItems
                .FirstOrDefaultAsync(i => i.Id == dto.Id, cancellationToken);

            if (item is null)
                throw new KeyNotFoundException($"ClaimItem {dto.Id} not found.");

            // Cập nhật scalar (chỉ đè khi DTO có giá trị)
            item.ClaimId = dto.ClaimId;
            if (dto.PartSerialNumber is not null) item.PartSerialNumber = dto.PartSerialNumber;
            if (dto.PayAmount.HasValue) item.PayAmount = dto.PayAmount;
            if (dto.PaidBy is not null) item.PaidBy = dto.PaidBy;
            if (dto.Note is not null) item.Note = dto.Note;
            if (dto.ImgURLs is not null) item.ImgURLs = dto.ImgURLs;

            if (!string.IsNullOrWhiteSpace(dto.Status) &&
                Enum.TryParse<ClaimItemStatus>(dto.Status, true, out var parsed))
            {
                item.Status = parsed;
            }


            await _context.SaveChangesAsync(cancellationToken);

            return new UpdateClaimItemResult(true);
        }
    }
}
