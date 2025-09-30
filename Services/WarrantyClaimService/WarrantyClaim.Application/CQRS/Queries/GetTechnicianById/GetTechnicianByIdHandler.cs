using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Queries.GetTechnicianById
{
    internal class GetTechnicianByIdHandler
        : IQueryHandler<GetTechnicianByIdQuery, GetTechnicianByIdResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetTechnicianByIdHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetTechnicianByIdResult> Handle(
            GetTechnicianByIdQuery request,
            CancellationToken cancellationToken)
        {
            var dto = await _context.Technicians
                .AsNoTracking()

                .Where(t => t.Id == request.TechnicianId)
                .Include(t => t.WorkOrders)
                .ProjectTo<TechnicianDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (dto is null)
                throw new KeyNotFoundException($"Technician {request.TechnicianId} not found.");

            return new GetTechnicianByIdResult(dto);
        }
    }
}
