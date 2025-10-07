using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Queries.GetClaimItemById
{
    public record GetClaimItemByIdQuery(Guid Id) : IQuery<GetClaimItemByIdResult>;

    public record GetClaimItemByIdResult(ClaimItemDto ClaimItem);
}
