using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Commands.DeleteSupplyPart
{
    internal class DeleteSupplyPartHandler
        : ICommandHandler<DeleteSupplyPartCommand, DeleteSupplyPartResult>
    {
        private readonly IApplicationDbContext _context;

        public DeleteSupplyPartHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DeleteSupplyPartResult> Handle(
            DeleteSupplyPartCommand request,
            CancellationToken cancellationToken)
        {
            // Tìm entity theo Id
            var entity = await _context.PartSupplies
                .FirstOrDefaultAsync(p => p.Id == request.PartSupplyId, cancellationToken);

            if (entity is null)
                throw new KeyNotFoundException($"PartSupply {request.PartSupplyId} not found.");

            _context.PartSupplies.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return new DeleteSupplyPartResult(true);
        }
    }
}
