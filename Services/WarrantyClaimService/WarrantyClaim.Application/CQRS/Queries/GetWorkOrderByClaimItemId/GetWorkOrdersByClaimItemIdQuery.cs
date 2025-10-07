using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Queries.GetWorkOrderByClaimItemId
{
    public record GetWorkOrdersByClaimItemIdQuery(Guid ClaimItemId)
        : IQuery<GetWorkOrdersByClaimItemIdResult>;

    public record GetWorkOrdersByClaimItemIdResult(List<WorkOrderDto> WorkOrders);
}
