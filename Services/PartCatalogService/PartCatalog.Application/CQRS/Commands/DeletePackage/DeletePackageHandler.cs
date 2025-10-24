using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using PartCatalog.Application.Data;

namespace PartCatalog.Application.CQRS.Commands.DeletePackage
{
    public class DeletePackageHandler
        : ICommandHandler<DeletePackageCommand, DeletePackageResult>
    {
        private readonly IApplicationDbContext _context;

        public DeletePackageHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DeletePackageResult> Handle(
            DeletePackageCommand request,
            CancellationToken cancellationToken)
        {
            var package = await _context.Packages
                .Include(p => p.Parts)
                .Include(p => p.WarrantyPolicies)
                .FirstOrDefaultAsync(p => p.PackageId == request.PackageId, cancellationToken);

            if (package == null)
                throw new KeyNotFoundException($"Package with Id {request.PackageId} not found.");

            if (package.Parts?.Any() == true)
                _context.Parts.RemoveRange(package.Parts);

            if (package.WarrantyPolicies?.Any() == true)
                _context.WarrantyPolicies.RemoveRange(package.WarrantyPolicies);

            _context.Packages.Remove(package);
            await _context.SaveChangesAsync(cancellationToken);

            return new DeletePackageResult(true);
        }
    }
}
