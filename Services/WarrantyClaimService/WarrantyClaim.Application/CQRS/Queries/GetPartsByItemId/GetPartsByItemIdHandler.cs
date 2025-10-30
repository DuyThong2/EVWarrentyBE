using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarrantyClaim.Application.Data;

namespace WarrantyClaim.Application.CQRS.Queries.GetPartsByItemId
{
    internal class GetPartsByItemIdHandler
        : IQueryHandler<GetPartsByItemIdQuery, GetPartsByItemIdResult>
    {
        private readonly IApplicationDbContext _db;
        private readonly IMapper _mapper;

        public GetPartsByItemIdHandler(IApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<GetPartsByItemIdResult> Handle(
            GetPartsByItemIdQuery request,
            CancellationToken cancellationToken)
        {
            var parts = await _db.PartSupplies
                .AsNoTracking()
                .Where(p => p.ClaimItemId == request.ClaimItemId)
                .ToListAsync(cancellationToken);


            var dtoList = _mapper.Map<List<PartSupplyDto>>(parts);

            return new GetPartsByItemIdResult(dtoList);
        }
    }
}
