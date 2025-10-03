using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using PartCatalog.Application.Data;

namespace PartCatalog.Application.CQRS.Commands.DeletePart
{
    public class DeletePartHandler
        : ICommandHandler<DeletePartCommand, DeletePartResult>
    {
        private readonly IApplicationDbContext _context;

        public DeletePartHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DeletePartResult> Handle(DeletePartCommand request, CancellationToken cancellationToken)
        {
            var part = await _context.Parts
                .FirstOrDefaultAsync(x => x.PartId == request.PartId, cancellationToken);

            if (part == null)
                return new DeletePartResult(false);

            _context.Parts.Remove(part);
            await _context.SaveChangesAsync(cancellationToken);

            return new DeletePartResult(true);
        }
    }
}
