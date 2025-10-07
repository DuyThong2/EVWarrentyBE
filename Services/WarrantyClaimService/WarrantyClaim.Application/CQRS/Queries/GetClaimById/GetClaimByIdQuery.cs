using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.WarrantyClaim.Queries.GetClaimById
{
    public record GetClaimByIdQuery(Guid Id) : IQuery<GetClaimByIdResult>;
    

    public record GetClaimByIdResult(ClaimDto Claim);
}
