using MediatR;
using Microsoft.EntityFrameworkCore;
using PartCatalog.Application.Data;
using PartCatalog.Application.CQRS.Queries.GetPackageById;

namespace PartCatalog.Application
{
    public class GetPackageByIdHandler : IRequestHandler<GetPackageByIdQuery, GetPackageByIdResult>
    {
        private readonly IApplicationDbContext _context;

        public GetPackageByIdHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetPackageByIdResult> Handle(GetPackageByIdQuery request, CancellationToken cancellationToken)
        {
            var package = await _context.Packages
                .Include(p => p.Parts) // load parts in package
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PackageId == request.PackageId, cancellationToken);

            if (package == null)
                throw new KeyNotFoundException($"Package with ID {request.PackageId} not found.");

            return new GetPackageByIdResult(package);
        }
    }
}
