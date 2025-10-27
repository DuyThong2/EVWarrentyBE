using BuildingBlocks.CQRS;
using PartCatalog.Application.Data;
using PartCatalog.Domain.Enums;
using PartCatalog.Domain.Models;

namespace PartCatalog.Application.CQRS.Commands.UpdatePart
{
    public class UpdatePartHandler
        : ICommandHandler<UpdatePartCommand, UpdatePartResult>
    {
        private readonly IApplicationDbContext _context;

        public UpdatePartHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UpdatePartResult> Handle(UpdatePartCommand request, CancellationToken cancellationToken)
        {
            var part = await _context.Parts
                .FirstOrDefaultAsync(x => x.PartId == request.PartId, cancellationToken);

            if (part == null)
                return new UpdatePartResult(false);

            var dto = request.Part;

            part.Name = dto.Name;
            part.Description = dto.Description;
            part.Price = dto.Price;
            part.Manufacturer = dto.Manufacturer;
            part.Unit = dto.Unit;
            part.SerialNumber = dto.SerialNumber;

            if (!string.IsNullOrWhiteSpace(dto.Status) && Enum.TryParse<ActiveStatus>(dto.Status, true, out var statusValue))
            {
                part.Status = statusValue;
            }

            part.CateId = dto.CategoryId;
            part.PackageId = dto.PackageId;

            _context.Parts.Update(part);
            await _context.SaveChangesAsync(cancellationToken);

            return new UpdatePartResult(true);
        }
    }
}
