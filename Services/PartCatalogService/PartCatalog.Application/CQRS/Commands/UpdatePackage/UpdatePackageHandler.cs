using MediatR;
using Microsoft.EntityFrameworkCore;
using PartCatalog.Application.Data;
using PartCatalog.Applications.UpdatePackage;

namespace PartCatalog.Application.Features.Packages.Handlers
{
    public class UpdatePackageHandler : IRequestHandler<UpdatePackageCommand, UpdatePackageResult>
    {
        private readonly IApplicationDbContext _context;

        public UpdatePackageHandler(IApplicationDbContext context)
        {
            _context = context;
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

            // Update fields
            entity.PackageCode = dto.PackageCode;
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.Model = dto.Model;
            entity.Status = dto.Status;
            entity.Quantity = dto.Quantity;
            entity.Note = dto.Note;
            entity.CategoryId = dto.CategoryId;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return new UpdatePackageResult(true, "Package updated successfully.");
        }
    }
}
