using AutoMapper;
using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using PartCatalog.Application.Data;

namespace PartCatalog.Application.CQRS.Commands.UpdateCategory
{
    public class UpdateCategoryHandler
        : ICommandHandler<UpdateCategoryCommand, UpdateCategoryResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateCategoryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UpdateCategoryResult> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Category;

            var entity = await _context.Categories
                .FirstOrDefaultAsync(c => c.CateId == dto.CateId, cancellationToken);

            if (entity == null)
                return new UpdateCategoryResult(false, "Category not found.");

            entity.CateCode = dto.CateCode;
            entity.CateName = dto.CateName;
            entity.Description = dto.Description;
            entity.Quantity = dto.Quantity;

            await _context.SaveChangesAsync(cancellationToken);

            return new UpdateCategoryResult(true, "Category updated successfully.");
        }
    }
}
