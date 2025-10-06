using MediatR;
using PartCatalog.Application.Commands.CreatePackage;
using PartCatalog.Application.Data;
using PartCatalog.Domain.Models;

namespace PartCatalog.Application.Features.Packages.Handlers
{
    public class CreatePackageHandler : IRequestHandler<CreatePackageCommand, CreatePackageResult>
    {
        private readonly IApplicationDbContext _context;

        public CreatePackageHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CreatePackageResult> Handle(CreatePackageCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Package;

            // Validate unique PackageCode
            if (_context.Packages.Any(p => p.PackageCode == dto.PackageCode))
            {
                return new CreatePackageResult(Guid.Empty, false, "PackageCode already exists.");
            }

            var entity = new Package
            {
                PackageId = Guid.NewGuid(),
                PackageCode = dto.PackageCode,
                Name = dto.Name,
                Description = dto.Description,
                Model = dto.Model,
                Status = dto.Status,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Quantity = dto.Quantity,
                Note = dto.Note,
                CategoryId = dto.CategoryId
            };

            _context.Packages.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return new CreatePackageResult(entity.PackageId, true, "Package created successfully.");
        }
    }
}
