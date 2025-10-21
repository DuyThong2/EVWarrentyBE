using BuildingBlocks.Messaging.Events;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PartCatalog.Application.Data;
using PartCatalog.Applications.UpdatePackage;

namespace PartCatalog.Application.Features.Packages.Handlers
{
    public class UpdatePackageHandler : IRequestHandler<UpdatePackageCommand, UpdatePackageResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;

        public UpdatePackageHandler(IApplicationDbContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<UpdatePackageResult> Handle(UpdatePackageCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Package;

            var entity = await _context.Packages
                .FirstOrDefaultAsync(p => p.PackageId == dto.PackageId, cancellationToken);

            if (entity == null)
            {
                return new UpdatePackageResult(false, "Package not found.");
            }

            var oldQuantity = entity.Quantity;

            // Update fields
            entity.PackageCode = dto.PackageCode;
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.Model = dto.Model;
            entity.Status = dto.Status;
            entity.Quantity = dto.Quantity;
            entity.Note = dto.Note;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.CategoryId = dto.CategoryId;
            if (dto.PartId != null)
            {

                var newParts = await _context.Parts
                    .Where(p => dto.PartId.Contains(p.PartId))
                    .ToListAsync(cancellationToken);

                entity.Parts?.Clear();
                entity.Parts = newParts;
            }

            await _context.SaveChangesAsync(cancellationToken);

            // Nếu Quantity thay đổi, publish event fanout
            if (entity.Quantity != oldQuantity)
            {
                var quantityChangedEvent = new PackageQuantityChangedEvent(
                    entity.PackageId,
                    oldQuantity,
                    entity.Quantity
                );

                await _publishEndpoint.Publish(quantityChangedEvent, cancellationToken);
            }

            return new UpdatePackageResult(true, "Package updated successfully.");
        }
    }
}
