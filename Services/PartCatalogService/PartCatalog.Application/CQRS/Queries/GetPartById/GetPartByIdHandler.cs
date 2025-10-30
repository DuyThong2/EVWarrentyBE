using BuildingBlocks.CQRS;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PartCatalog.Application.Data;
using PartCatalog.Application.DTOs;

namespace PartCatalog.Application.CQRS.Queries.GetPartById
{
    public class GetPartByIdHandler
        : IQueryHandler<GetPartByIdQuery, GetPartByIdResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPartByIdHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetPartByIdResult> Handle(GetPartByIdQuery request, CancellationToken cancellationToken)
        {
            var part = await _context.Parts
                .AsNoTracking()
                .Include(x => x.Category)
                .Include(x => x.Package)
                    .ThenInclude(p => p.Category)
                .FirstOrDefaultAsync(x => x.PartId == request.Id, cancellationToken);

            if (part == null)
                throw new KeyNotFoundException($"Part with id {request.Id} not found.");

            var dto = _mapper.Map<PartDto>(part);

            return new GetPartByIdResult(dto);
        }
    }
}
