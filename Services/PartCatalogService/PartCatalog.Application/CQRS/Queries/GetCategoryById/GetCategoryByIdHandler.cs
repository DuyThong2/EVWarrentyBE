using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using PartCatalog.Application.Data;
using PartCatalog.Application.DTOs;
using AutoMapper;

namespace PartCatalog.Application.CQRS.Queries.GetCategoryById
{
    public class GetCategoryByIdHandler
        : IQueryHandler<GetCategoryByIdQuery, GetCategoryByIdResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCategoryByIdHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetCategoryByIdResult> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Categories
                .Include(c => c.Parts)
                .Include(c => c.Packages)
                .FirstOrDefaultAsync(c => c.CateId == request.CateId, cancellationToken);

            if (entity == null)
                throw new KeyNotFoundException($"Category with ID {request.CateId} not found.");

            var dto = _mapper.Map<CategoryDto>(entity);

            return new GetCategoryByIdResult(dto);
        }
    }

    public record GetCategoryByIdResult(CategoryDto Category);
}
