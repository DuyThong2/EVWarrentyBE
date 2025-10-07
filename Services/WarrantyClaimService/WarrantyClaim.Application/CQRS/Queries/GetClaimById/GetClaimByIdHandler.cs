using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarrantyClaim.Application.Data;
using WarrantyClaim.Application.Dtos;
using WarrantyClaim.Application.Exceptions;
using WarrantyClaim.Application.WarrantyClaim.Queries.GetClaimById;

namespace WarrantyClaim.Application.CQRS.Queries.GetClaimById
{
    internal class GetClaimByIdHandler
        : IQueryHandler<GetClaimByIdQuery, GetClaimByIdResult>
    {
        private readonly IApplicationDbContext _db;
        private readonly IMapper _mapper;

        public GetClaimByIdHandler(IApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<GetClaimByIdResult> Handle(GetClaimByIdQuery request, CancellationToken cancellationToken)
        {
            var claim = await _db.Claims
                            .AsNoTracking()
                            .Include(c => c.Technician)
                            .Include(c => c.Items)
                            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (claim is null)
                throw new ClaimNotFoundException(request.Id);

            var claimDto = _mapper.Map<ClaimDto>(claim);

            return new GetClaimByIdResult(claimDto);
        }
    }
}
