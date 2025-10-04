using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Queries.GetWorkOrderByClaimItemId
{
    internal class GetWorkOrdersByClaimItemIdHandler
        : IQueryHandler<GetWorkOrdersByClaimItemIdQuery, GetWorkOrdersByClaimItemIdResult>
    {
        private readonly IApplicationDbContext _db;
        private readonly IMapper _mapper;

        public GetWorkOrdersByClaimItemIdHandler(IApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<GetWorkOrdersByClaimItemIdResult> Handle(
            GetWorkOrdersByClaimItemIdQuery request,
            CancellationToken cancellationToken)
        {
            var workOrders = await _db.WorkOrders
                .AsNoTracking()
                .Include(w => w.Technician)                 
                .Where(w => w.ClaimItemId == request.ClaimItemId)
                .ToListAsync(cancellationToken);

            var dtoList = _mapper.Map<List<WorkOrderDto>>(workOrders);

            return new GetWorkOrdersByClaimItemIdResult(dtoList);
        }
    }
}
