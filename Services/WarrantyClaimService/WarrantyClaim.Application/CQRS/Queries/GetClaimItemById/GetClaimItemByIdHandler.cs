using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarrantyClaim.Application.Data;
using WarrantyClaim.Application.Exceptions;

namespace WarrantyClaim.Application.CQRS.Queries.GetClaimItemById
{
    internal class GetClaimItemByIdHandler
        : IQueryHandler<GetClaimItemByIdQuery, GetClaimItemByIdResult>
    {
        private readonly IApplicationDbContext _db;
        private readonly IMapper _mapper;

        public GetClaimItemByIdHandler(IApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<GetClaimItemByIdResult> Handle(GetClaimItemByIdQuery request, CancellationToken cancellationToken)
        {
            var claimItem = await _db.ClaimItems
                .AsNoTracking()
                .Include(i => i.PartSupplies)
                .Include(i => i.WorkOrders)
                    .ThenInclude(w => w.Technician)
                .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (claimItem is null)
                throw new ClaimItemNotFoundException(request.Id);

            var dto = _mapper.Map<ClaimItemDto>(claimItem);

            return new GetClaimItemByIdResult(dto);
        }
    }
}
