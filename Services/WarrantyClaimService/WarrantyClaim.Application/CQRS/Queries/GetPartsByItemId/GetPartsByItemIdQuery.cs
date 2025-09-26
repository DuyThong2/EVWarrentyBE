using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Queries.GetPartsByItemId
{
    public record GetPartsByItemIdQuery(Guid ClaimItemId)
        : IQuery<GetPartsByItemIdResult>;

    public record GetPartsByItemIdResult(List<PartSupplyDto> Parts);
}
