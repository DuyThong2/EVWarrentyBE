using AutoMapper;
using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using PartCatalog.Application.Data;
using PartCatalog.Domain.Models;

namespace PartCatalog.Application.CQRS.Commands.CreateCategory
{
    public class CreateCategoryHandler
        : ICommandHandler<CreateCategoryCommand, CreateCategoryResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateCategoryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CreateCategoryResult> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Category;

            // Check duplicate CateCode
            var duplicate = await _context.Categories
                .AnyAsync(c => c.CateCode == dto.CateCode, cancellationToken);

            if (duplicate)
                return new CreateCategoryResult(Guid.Empty, false, "Category code already exists.");

            var entity = _mapper.Map<Category>(dto);
            entity.CateId = Guid.NewGuid();

            await _context.Categories.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new CreateCategoryResult(entity.CateId, true, "Category created successfully.");
        }
    }
}
