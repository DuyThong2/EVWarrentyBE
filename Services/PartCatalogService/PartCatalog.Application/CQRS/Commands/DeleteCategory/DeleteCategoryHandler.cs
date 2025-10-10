using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using PartCatalog.Application.Data;

namespace PartCatalog.Application.CQRS.Commands.DeleteCategory
{
    public class DeleteCategoryHandler
        : ICommandHandler<DeleteCategoryCommand, DeleteCategoryResult>
    {
        private readonly IApplicationDbContext _context;

        public DeleteCategoryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DeleteCategoryResult> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Categories
                .FirstOrDefaultAsync(c => c.CateId == request.CateId, cancellationToken);

            if (entity == null)
                return new DeleteCategoryResult(false, "Category not found.");

            _context.Categories.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return new DeleteCategoryResult(true, "Category deleted successfully.");
        }
    }
}
