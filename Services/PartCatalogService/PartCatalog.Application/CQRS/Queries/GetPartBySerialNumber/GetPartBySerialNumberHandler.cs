using AutoMapper;
using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using PartCatalog.Application.Data;
using PartCatalog.Application.DTOs;

namespace PartCatalog.Application.CQRS.Queries.GetPartBySerialNumber
{
    public class GetPartBySerialNumberHandler
        : IQueryHandler<GetPartBySerialNumberQuery, GetPartBySerialNumberResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPartBySerialNumberHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetPartBySerialNumberResult> Handle(
            GetPartBySerialNumberQuery request,
            CancellationToken cancellationToken)
        {
            var part = await _context.Parts
                .FirstOrDefaultAsync(p => p.SerialNumber == request.SerialNumber, cancellationToken);

            if (part == null)
                return new GetPartBySerialNumberResult(null);

            var dto = _mapper.Map<PartDto>(part);
            return new GetPartBySerialNumberResult(dto);
        }
    }
}
